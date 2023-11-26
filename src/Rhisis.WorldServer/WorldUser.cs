using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhisis.WorldServer;

public sealed class WorldUser : FFUserConnection
{
    private readonly IServiceProvider _serviceProvider;

    internal Player Player { get; set; }

    public WorldUser(ILogger<WorldUser> logger, IServiceProvider serviceProvider) 
        : base(logger)
    {
        _serviceProvider = serviceProvider;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        if (Socket is null)
        {
            Logger.LogTrace("Skip to handle login packet. Reason: client is not connected.");
            return Task.CompletedTask;
        }

        try
        {
            // We must skip the first 4 bytes because it represents the DPID which is always 0xFFFFFFFF (uint.MaxValue)
            byte[] packetBufferArray = packetBuffer.Skip(4).ToArray();
            FFPacket packet = new(packetBufferArray);
            PacketDispatcher.Execute(this, packet.Header, packet, _serviceProvider);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occured while handling a world packet.");
        }

        return base.HandleMessageAsync(packetBuffer);
    }

    public void SavePlayer()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        IGameDatabase gameDatabase = services.GetRequiredService<IGameDatabase>();

        SavePlayerInformation(Player, gameDatabase);
        SavePlayerInventory(Player, gameDatabase);
        SavePlayerQuests(Player, gameDatabase);
        SavePlayerSkills(Player, gameDatabase);
        SavePlayerBuffs(Player, gameDatabase);
    }

    protected override void OnDisconnected()
    {
        // TODO: notify cluster and disconnect from messenger
        SavePlayer();
        Player.Dispose();
        Player = null;

        base.OnDisconnected();
    }

    private static void SavePlayerInformation(Player player, IGameDatabase gameDatabase)
    {
        PlayerEntity playerEntity = gameDatabase.Players.FirstOrDefault(x => x.Id == player.Id) 
            ?? throw new InvalidOperationException($"Cannot find player with id: {player.Id} in database (Player: {player.Name})");

        playerEntity.Level = player.Level;
        playerEntity.Experience = player.Experience.Amount;
        playerEntity.Angle = player.RotationAngle;

        playerEntity.PosX = player.Position.X;
        playerEntity.PosY = player.Position.Y; 
        playerEntity.PosZ = player.Position.Z;
        playerEntity.MapId = player.Map.Id;
        playerEntity.MapLayerId = player.MapLayer.Id;
        playerEntity.Strength = player.Statistics.Strength;
        playerEntity.Stamina = player.Statistics.Stamina;
        playerEntity.Dexterity = player.Statistics.Dexterity;
        playerEntity.Intelligence = player.Statistics.Intelligence;
        playerEntity.StatPoints = player.AvailablePoints;
        playerEntity.SkillPoints = player.SkillPoints;
        playerEntity.Hp = player.Health.Hp;
        playerEntity.Mp = player.Health.Mp;
        playerEntity.Fp = player.Health.Fp;
        playerEntity.FaceId = player.Appearence.FaceId;
        playerEntity.SkinSetId = player.Appearence.SkinSetId;
        playerEntity.HairId = player.Appearence.HairId;
        playerEntity.HairColor = player.Appearence.HairColor;
        playerEntity.Gender = (byte)player.Appearence.Gender;
        playerEntity.Gold = player.Gold.Amount;
        playerEntity.JobId = (int)player.Job.Id;
        playerEntity.Slot = (byte)player.Slot;
        playerEntity.BankCode = player.BankCode;

        gameDatabase.Players.Update(playerEntity);
        gameDatabase.SaveChanges();
    }

    private static void SavePlayerInventory(Player player, IGameDatabase gameDatabase)
    {
        List<PlayerItemEntity> playerItems = gameDatabase.PlayerItems
            .Where(x => x.PlayerId == player.Id && x.StorageType == PlayerItemStorageType.Inventory)
            .ToList();

        foreach (PlayerItemEntity itemToRemove in playerItems)
        {
            gameDatabase.PlayerItems.Remove(itemToRemove);
        }

        for (int i = 0; i < player.Inventory.MaxCapacity; i++)
        {
            ItemContainerSlot slot = player.Inventory.GetAtSlot(i);

            if (slot.HasItem)
            {
                ItemEntity item = gameDatabase.Items.FirstOrDefault(x => x.SerialNumber == slot.Item.SerialNumber) ?? new ItemEntity();

                item.Refine = slot.Item.Refine;
                item.Element = (byte)slot.Item.Element;
                item.ElementRefine = slot.Item.ElementRefine;
                // TODO: save item attributes like awakening statistics.

                if (item.SerialNumber == 0)
                {
                    item.Id = slot.Item.Id;
                    //gameDatabase.Items.Add(item);
                }

                PlayerItemEntity playerItem = new()
                {
                    PlayerId = player.Id,
                    Slot = (byte)slot.Number,
                    Quantity = slot.Item.Quantity,
                    StorageType = PlayerItemStorageType.Inventory,
                    Item = item
                };

                gameDatabase.PlayerItems.Add(playerItem);
            }
        }

        gameDatabase.SaveChanges();
    }

    private static void SavePlayerQuests(Player player, IGameDatabase gameDatabase)
    {
        var questSet = from quest in player.QuestDiary
                       join questInDatabase in gameDatabase.PlayerQuests.Where(x => x.PlayerId == player.Id).ToList()
                       on new { QuestId = quest.Id, PlayerId = player.Id } equals new { questInDatabase.QuestId, questInDatabase.PlayerId } into d
                       from q in d.DefaultIfEmpty()
                       select new
                       {
                           Quest = quest,
                           DatabaseQuest = q,
                           IsNew = q is null
                       };

        foreach (var q in questSet)
        {
            if (q.IsNew)
            {
                PlayerQuestEntity quest = new()
                {
                    PlayerId = player.Id,
                    QuestId = q.Quest.Id,
                    StartTime = q.Quest.StartTime,
                    EndTime = q.Quest.EndTime,
                    Finished = q.Quest.IsFinished,
                    IsChecked = q.Quest.IsChecked,
                    IsDeleted = q.Quest.IsDeleted,
                    IsPatrolDone = q.Quest.IsPatrolDone,
                    MonsterKilled1 = q.Quest.Monsters?.ElementAtOrDefault(0).Value ?? 0,
                    MonsterKilled2 = q.Quest.Monsters?.ElementAtOrDefault(1).Value ?? 0
                };

                gameDatabase.PlayerQuests.Add(quest);
            }
            else
            {
                q.DatabaseQuest.StartTime = q.Quest.StartTime;
                q.DatabaseQuest.EndTime = q.Quest.EndTime;
                q.DatabaseQuest.Finished = q.Quest.IsFinished;
                q.DatabaseQuest.IsChecked = q.Quest.IsChecked;
                q.DatabaseQuest.IsDeleted = q.Quest.IsDeleted;
                q.DatabaseQuest.IsPatrolDone = q.Quest.IsPatrolDone;
                q.DatabaseQuest.MonsterKilled1 = q.Quest.Monsters?.ElementAtOrDefault(0).Value ?? 0;
                q.DatabaseQuest.MonsterKilled2 = q.Quest.Monsters?.ElementAtOrDefault(1).Value ?? 0;

                gameDatabase.PlayerQuests.Update(q.DatabaseQuest);
            }
        }

        gameDatabase.SaveChanges();
    }

    private static void SavePlayerSkills(Player player, IGameDatabase gameDatabase)
    {
        IEnumerable<PlayerSkillEntity> playerSkills = gameDatabase.PlayerSkills
            .Where(x => x.PlayerId == player.Id)
            .ToList();

        foreach (PlayerSkillEntity skill in playerSkills)
        {
            gameDatabase.PlayerSkills.Remove(skill);
        }

        playerSkills = player.Skills.Select(x => new PlayerSkillEntity
        {
            SkillId = x.Id,
            SkillLevel = x.Level,
            PlayerId = player.Id
        });

        foreach (PlayerSkillEntity skill in playerSkills)
        {
            gameDatabase.PlayerSkills.Add(skill);
        }

        gameDatabase.SaveChanges();
    }

    private static void SavePlayerBuffs(Player player, IGameDatabase gameDatabase)
    {
        IEnumerable<PlayerSkillBuffAttributeEntity> buffAttributes = gameDatabase.PlayerSkillBuffAttributes.Where(x => x.PlayerId == player.Id);
        IEnumerable<PlayerSkillBuffEntity> buffs = gameDatabase.PlayerSkillBuffs.Where(x => x.PlayerId == player.Id);

        foreach (PlayerSkillBuffAttributeEntity buffAttribute in buffAttributes)
        {
            gameDatabase.PlayerSkillBuffAttributes.Remove(buffAttribute);
        }
        
        foreach (PlayerSkillBuffEntity skillBuff in buffs)
        {
            gameDatabase.PlayerSkillBuffs.Remove(skillBuff);
        }

        foreach (BuffSkill buff in player.Buffs.OfType<BuffSkill>())
        {
            PlayerSkillBuffEntity skillBuff = new()
            {
                PlayerId = player.Id,
                SkillId = buff.SkillId,
                SkillLevel = buff.SkillLevel,
                RemainingTime = buff.RemainingTime,
                Attributes = buff.Attributes.Select(x => new PlayerSkillBuffAttributeEntity()
                {
                    SkillId = buff.SkillId,
                    PlayerId = player.Id,
                    Attribute = x.Key,
                    Value = x.Value
                }).ToList()
            };

            gameDatabase.PlayerSkillBuffs.Add(skillBuff);
        }

        gameDatabase.SaveChanges();
    }
}
