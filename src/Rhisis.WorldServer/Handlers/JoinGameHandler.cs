using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Game.Protocol.Packets.World.Server;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties.Quests;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Handlers;

[PacketHandler(PacketType.JOIN)]
internal class JoinGameHandler : WorldPacketHandler
{
    private readonly ILogger<JoinGameHandler> _logger;
    private readonly IAccountDatabase _accountDatabase;
    private readonly IGameDatabase _gameDatabase;

    public JoinGameHandler(ILogger<JoinGameHandler> logger, IAccountDatabase accountDatabase, IGameDatabase gameDatabase)
    {
        _logger = logger;
        _accountDatabase = accountDatabase;
        _gameDatabase = gameDatabase;
    }

    public void Execute(JoinPacket packet)
    {
        AccountEntity userAccount = _accountDatabase.Accounts.SingleOrDefault(x => x.Username == packet.Username && x.Password == packet.Password);

        if (userAccount is null)
        {
            _logger.LogWarning($"Unable to join for user '{packet.Username}' Reason: bad presented credentials compared to the database.");
            User.Disconnect();

            return;
        }

        PlayerEntity player = _gameDatabase.Players.SingleOrDefault(x => x.AccountId == userAccount.Id && x.Id == packet.PlayerId && x.Name == packet.PlayerName);

        if (player is null)
        {
            _logger.LogWarning($"Unable to join for user '{packet.Username}' Reason: Cannot find player with id: '{packet.PlayerId}' and name: '{packet.PlayerName}'.");
            User.Disconnect();

            return;
        }

        if (player.IsDeleted)
        {
            _logger.LogWarning($"Unable to join for user '{packet.Username}' Reason: player '{player.Name}' is deleted.");
            User.Disconnect();

            return;
        }

        int modelId = player.Gender == 0 ? 11 : 12;
        Map map = MapManager.Current.Get(player.MapId) ?? throw new InvalidOperationException($"Failed to find map with id: '{player.MapId}'.");
        MapLayer layer = map.GetLayer(player.MapLayerId) ?? map.GetDefaultLayer();

        User.Player = new Player(User, GameResources.Current.Movers.Get(modelId))
        {
            Id = player.Id,
            Name = player.Name,
            Slot = player.Slot,
            DeathLevel = 0,
            Authority = (AuthorityType)userAccount.Authority,
            Position = new Vector3(player.PosX, player.PosY, player.PosZ),
            Map = map,
            MapLayer = layer,
            RotationAngle = player.Angle,
            Level = player.Level,
            ModelId = modelId,
            ObjectState = ObjectState.OBJSTA_STAND,
            Job = GameResources.Current.Jobs.Get(player.JobId),
            AvailablePoints = player.StatPoints,
            SkillPoints = (ushort)player.SkillPoints,
            Appearence = new HumanVisualAppearance
            {
                Gender = player.Gender == 0 ? GenderType.Male : GenderType.Female,
                SkinSetId = player.SkinSetId,
                FaceId = player.FaceId,
                HairColor = player.HairColor,
                HairId = player.HairId,
            },
            BankCode = player.BankCode,
        };
        User.Player.Health.Hp = player.Hp;
        User.Player.Health.Mp = player.Mp;
        User.Player.Health.Fp = player.Fp;

        User.Player.Statistics.Strength = player.Strength;
        User.Player.Statistics.Stamina = player.Stamina;
        User.Player.Statistics.Dexterity = player.Dexterity;
        User.Player.Statistics.Intelligence = player.Intelligence;

        User.Player.Gold.Initialize(player.Gold);
        User.Player.Experience.Initialize(player.Experience);


        LoadInventory(User.Player, _gameDatabase);
        LoadSkills(User.Player, _gameDatabase);
        LoadQuests(User.Player, _gameDatabase);
        LoadBuffs(User.Player, _gameDatabase);

        User.Player.Defense.Update();

        // TODO: if dead, apply penality and revive to nearest region

        using (JoinCompletePacket joinPacket = new())
        {
            joinPacket.AddSnapshots(
                new EnvironmentAllSnapshot(User.Player, SeasonType.None), // TODO: get the season id using current weather time.
                new WorldReadInfoSnapshot(User.Player),
                new AddObjectSnapshot(User.Player),
                new TaskbarSnapshot(User.Player)
                //new QueryPlayerDataSnapshot(cachedPlayer),
                //new AddFriendGameJoinSnapshot(User.Player)
            );

            User.Send(joinPacket);
        }

        layer.AddPlayer(User.Player);

        User.Player.IsSpawned = true;
    }

    private static void LoadInventory(Player player, IGameDatabase gameDatabase)
    {
        Dictionary<int, Item> playerInventoryItems = gameDatabase.PlayerItems
            .Include(x => x.Item)
            .Where(x => x.PlayerId == player.Id && x.StorageType == PlayerItemStorageType.Inventory)
            .ToDictionary(x => (int)x.Slot,
                x => new Item(GameResources.Current.Items.Get(x.Item.Id))
                {
                    SerialNumber = x.Item.SerialNumber,
                    Refine = x.Item.Refine.GetValueOrDefault(0),
                    Element = (ElementType)x.Item.Element.GetValueOrDefault(0),
                    ElementRefine = x.Item.ElementRefine.GetValueOrDefault(0),
                    Quantity = x.Quantity
                });

        if (playerInventoryItems.Any())
        {
            player.Inventory.Initialize(playerInventoryItems);
        }
    }

    private static void LoadSkills(Player player, IGameDatabase gameDatabase)
    {
        IEnumerable<PlayerSkillEntity> playerSkills = gameDatabase.PlayerSkills
            .Where(x => x.PlayerId == player.Id)
            .ToList();

        IEnumerable<Skill> skills = from skill in GameResources.Current.Skills.GetJobSkills(player.Job.Id)
                                    join playerSkill in playerSkills on skill.Id equals playerSkill.SkillId into sj
                                    from s in sj.DefaultIfEmpty()
                                    select new Skill(skill, player, s?.SkillLevel ?? 0);

        player.Skills.SetSkills(skills);
    }

    private static void LoadQuests(Player player, IGameDatabase gameDatabase)
    {
        IEnumerable<Quest> playerQuests = gameDatabase.PlayerQuests
            .Where(x => x.PlayerId == player.Id)
            .ToList()
            .Select(x =>
            {
                QuestProperties questProperties = GameResources.Current.Quests.Get(x.QuestId);
                Quest quest = new(questProperties, player)
                {
                    IsChecked = x.IsChecked,
                    IsFinished = x.Finished,
                    StartTime = x.StartTime,
                    IsPatrolDone = x.IsPatrolDone
                };

                if (quest.Properties.QuestEndCondition.Monsters is not null && quest.Properties.QuestEndCondition.Monsters.Any())
                {
                    quest.Monsters = new Dictionary<int, short>
                    {
                        { GameResources.Current.GetDefinedValue(quest.Properties.QuestEndCondition.Monsters.ElementAtOrDefault(0)?.Id), (short)x.MonsterKilled1 },
                        { GameResources.Current.GetDefinedValue(quest.Properties.QuestEndCondition.Monsters.ElementAtOrDefault(1)?.Id), (short)x.MonsterKilled2 }
                    };
                }

                return quest;
            });

        player.QuestDiary.SetQuests(playerQuests);
    }

    private static void LoadBuffs(Player player, IGameDatabase gameDatabase)
    {
        IEnumerable<PlayerSkillBuffEntity> buffs = gameDatabase.PlayerSkillBuffs
            .Include(x => x.Attributes)
            .Where(x => x.PlayerId == player.Id)
            .ToList();

        foreach (PlayerSkillBuffEntity buff in buffs)
        {
            BuffSkill buffSkill = new(
                owner: player,
                attributes: buff.Attributes.ToDictionary(x => x.Attribute, x => x.Value),
                skillProperties: GameResources.Current.Skills.Get(buff.SkillId),
                skillLevel: buff.SkillLevel)
            {
                RemainingTime = buff.RemainingTime
            };

            player.Buffs.Add(buffSkill);
        }
    }
}