using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Resources;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.Cluster;
using Rhisis.Protocol.Packets.Server;
using Rhisis.Protocol.Packets.Server.Cluster;
using Sylver.HandlerInvoker.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers;

[Handler]
public class CreatePlayerHandler : ClusterHandlerBase
{
    private readonly ILogger<CreatePlayerHandler> _logger;
    private readonly IOptions<ClusterOptions> _clusterOptions;
    private readonly IGameResources _gameResources;

    public CreatePlayerHandler(ILogger<CreatePlayerHandler> logger, IOptions<ClusterOptions> clusterOptions, IRhisisDatabase database, IGameResources gameResources)
        : base(database)
    {
        _logger = logger;
        _clusterOptions = clusterOptions;
        _gameResources = gameResources;
    }

    [HandlerAction(PacketType.CREATE_PLAYER)]
    public void OnCreatePlayer(IClusterUser user, CreatePlayerPacket packet)
    {
        DbUser dbUser = Database.Users.FirstOrDefault(x => x.Username == packet.Username && x.Password == packet.Password);

        if (dbUser is null)
        {
            _logger.LogWarning($"[SECURITY] Unable to create new character for user '{packet.Username}' " +
                "Reason: bad presented credentials compared to the database.");
            user.Disconnect();
            return;
        }

        if (Database.Characters.Any(x => x.Name == packet.CharacterName))
        {
            _logger.LogWarning(
                    $"Unable to create new character for user '{packet.Username}' " +
                    $"Reason: character name '{packet.CharacterName}' already exists.");

            SendError(user, ErrorType.USER_EXISTS);

            return;
        }

        DefaultCharacter defaultCharacter = _clusterOptions.Value.DefaultCharacter;
        DefaultStartItems defaultEquipment = packet.Gender == 0 ? defaultCharacter.Man : defaultCharacter.Woman;

        if (!_gameResources.Jobs.TryGetValue(packet.Job, out JobData jobData))
        {
            _logger.LogError($"Cannot find job data for job '{packet.Job}' for user '{dbUser.Username}'.");
            user.Disconnect();
            return;
        }

        var newCharacter = new DbCharacter()
        {
            UserId = dbUser.Id,
            Name = packet.CharacterName,
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
            Strength = defaultCharacter.Strength,
            Stamina = defaultCharacter.Stamina,
            Dexterity = defaultCharacter.Dexterity,
            Intelligence = defaultCharacter.Intelligence,
            MapId = defaultCharacter.MapId,
            PosX = defaultCharacter.PosX,
            PosY = defaultCharacter.PosY,
            PosZ = defaultCharacter.PosZ,
            Level = defaultCharacter.Level,
            Gold = defaultCharacter.Gold,
            StatPoints = 0, //TODO: create default stat point constant.
            SkillPoints = 0, //TODO: create default skill point constant.
            Experience = 0,
            ClusterId = _clusterOptions.Value.Id
        };

        //TODO: create game constants for slot.
        newCharacter.Items.Add(new DbItemStorage
        {
            StorageTypeId = (int)ItemStorageType.Inventory,
            Quantity = 1,
            Item = new DbItem
            {
                GameItemId = defaultEquipment.StartSuit
            },
            Slot = 44
        });
        newCharacter.Items.Add(new DbItemStorage
        {
            StorageTypeId = (int)ItemStorageType.Inventory,
            Quantity = 1,
            Item = new DbItem
            {
                GameItemId = defaultEquipment.StartHand
            },
            Slot = 46
        });
        newCharacter.Items.Add(new DbItemStorage
        {
            StorageTypeId = (int)ItemStorageType.Inventory,
            Quantity = 1,
            Item = new DbItem
            {
                GameItemId = defaultEquipment.StartShoes
            },
            Slot = 47
        });
        newCharacter.Items.Add(new DbItemStorage
        {
            StorageTypeId = (int)ItemStorageType.Inventory,
            Quantity = 1,
            Item = new DbItem
            {
                GameItemId = defaultEquipment.StartWeapon
            },
            Slot = 52
        });

        Database.Characters.Add(newCharacter);
        Database.SaveChanges();

        _logger.LogInformation($"Character '{newCharacter.Name}' has been created successfully for user '{dbUser.Username}'.");

        SendPlayerList(user, packet.AuthenticationKey);
    }
}
