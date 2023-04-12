using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Configuration.Cluster;
using Rhisis.Core.Extensions;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.Cluster.Client;
using Rhisis.Game.Resources;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.CREATE_PLAYER)]
internal class CreatePlayerHandler : ClusterHandlerBase
{
    private static readonly byte InventoryMaxSize = 42;
    private readonly ILogger<CreatePlayerPacket> _logger;
    private readonly IAccountDatabase _accountDatabase;
    private readonly IGameDatabase _gameDatabase;
    private readonly ICluster _cluster;

    public CreatePlayerHandler(ILogger<CreatePlayerPacket> logger, IAccountDatabase accountDatabase, IGameDatabase gameDatabase, ICluster cluster)
    {
        _logger = logger;
        _accountDatabase = accountDatabase;
        _gameDatabase = gameDatabase;
        _cluster = cluster;
    }

    public void Execute(CreatePlayerPacket packet)
    {
        AccountEntity userAccount = _accountDatabase.Accounts.SingleOrDefault(x => x.Username == packet.Username && x.Password == packet.Password && x.Id == User.AccountId);

        if (userAccount is null)
        {
            _logger.LogWarning($"[SECURITY] Unable to create new character for user '{packet.Username}' Reason: bad presented credentials compared to the database.");
            User.Disconnect();

            return;
        }

        if (_gameDatabase.Players.Any(x => x.Name.ToLower() == packet.CharacterName.ToLower()))
        {
            _logger.LogWarning(
                    $"Unable to create new character for user '{packet.Username}' " +
                    $"Reason: character name '{packet.CharacterName}' already exists.");
            User.SendError(ErrorType.USER_EXISTS);

            return;
        }

        DefaultCharacterOptions defaultPlayerOptions = packet.Gender == (byte)GenderType.Male ? 
            _cluster.Configuration.DefaultCharacter.Man : _cluster.Configuration.DefaultCharacter.Woman;

        PlayerEntity newPlayer = new()
        {
            AccountId = userAccount.Id,
            Name = packet.CharacterName.TakeCharacters(32),
            Slot = (byte)packet.Slot,
            SkinSetId = packet.SkinSet,
            HairColor = (int)packet.HairColor,
            FaceId = packet.HeadMesh,
            HairId = packet.HairMeshId,
            BankCode = packet.BankPassword,
            Gender = packet.Gender,
            JobId = 0,
            // Not the best of solutions.
            // Need to store these values in a configuration file of isolate the health calculation
            // in a shared injectable service in both cluster and world server.
            Hp = 100,
            Mp = 50,
            Fp = 50,
            Strength = defaultPlayerOptions.Strength,
            Stamina = defaultPlayerOptions.Stamina,
            Dexterity = defaultPlayerOptions.Dexterity,
            Intelligence = defaultPlayerOptions.Intelligence,
            MapId = defaultPlayerOptions.MapId,
            PosX = defaultPlayerOptions.PositionX,
            PosY = defaultPlayerOptions.PositionY,
            PosZ = defaultPlayerOptions.PositionZ,
            Level = defaultPlayerOptions.Level,
            Gold = defaultPlayerOptions.Gold,
            StatPoints = 0, //TODO: create default stat point constant.
            SkillPoints = 0, //TODO: create default skill point constant.
            Experience = 0
        };

        CreatePlayerItem(newPlayer, defaultPlayerOptions.EquipedItems.Hat, ItemPartType.Hat);
        CreatePlayerItem(newPlayer, defaultPlayerOptions.EquipedItems.Body, ItemPartType.UpperBody);
        CreatePlayerItem(newPlayer, defaultPlayerOptions.EquipedItems.Hand, ItemPartType.Hand);
        CreatePlayerItem(newPlayer, defaultPlayerOptions.EquipedItems.RightWeapon, ItemPartType.RightWeapon);
        CreatePlayerItem(newPlayer, defaultPlayerOptions.EquipedItems.LeftWeapon, ItemPartType.LeftWeapon);
        CreatePlayerItem(newPlayer, defaultPlayerOptions.EquipedItems.Boots, ItemPartType.Foot);

        if (defaultPlayerOptions.InventoryItems.Any())
        {
            for (int i = 0; i < defaultPlayerOptions.InventoryItems.Count() && i < InventoryMaxSize; i++)
            {
                DefaultInventoryItem item = defaultPlayerOptions.InventoryItems.ElementAt(i);

                CreatePlayerItem(newPlayer, item.ItemId, (byte)i, item.Quantity, item.Refine, item.Element, item.ElementRefine);
            }
        }

        _gameDatabase.Players.Add(newPlayer);
        _gameDatabase.SaveChanges();

        _logger.LogInformation($"Player '{newPlayer.Name}' has been created successfully for user '{userAccount.Username}'.");
        User.SendPlayerList();
    }

    private static void CreatePlayerItem(PlayerEntity player, string itemIdentifier, byte slot, int quantity = 1, byte refine = 0, byte element = 0, byte elementRefine = 0)
    {
        int itemId = GameResources.Current.Items.Get(itemIdentifier)?.Id ?? -1;

        if (itemId > 0)
        {
            player.Items.Add(new PlayerItemEntity
            {
                StorageType = PlayerItemStorageType.Inventory,
                Slot = slot,
                Quantity = quantity,
                Item = new ItemEntity
                {
                    Id = itemId,
                    Refine = refine,
                    Element = element,
                    ElementRefine = elementRefine
                }
            });
        }
    }

    private static void CreatePlayerItem(PlayerEntity player, string itemIdentifier, ItemPartType partType)
    {
        CreatePlayerItem(player, itemIdentifier, (byte)((byte)partType + InventoryMaxSize));
    }
}
