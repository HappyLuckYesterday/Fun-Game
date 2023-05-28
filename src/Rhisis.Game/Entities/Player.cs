using Rhisis.Core.Configuration.Cluster;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Skills;
using Rhisis.Game.Resources.Properties;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Rhisis.Game.Entities;

[DebuggerDisplay("{Name} Lv.{Level} ({Job.Id})")]
public sealed class Player : Mover
{
    private readonly FFUserConnection _connection;

    /// <summary>
    /// Gets the player's id.
    /// </summary>
    /// <remarks>
    /// This id is the player's id as stored in the database.
    /// </remarks>
    public int Id { get; init; }

    /// <summary>
    /// Gets the player login date.
    /// </summary>
    public DateTime LoggedInAt { get; init; }

    /// <summary>
    /// Gets the player slot.
    /// </summary>
    public int Slot { get; init; }

    /// <summary>
    /// Gets the player authority.
    /// </summary>
    public AuthorityType Authority { get; init; }

    /// <summary>
    /// Gets or sets the player's job.
    /// </summary>
    public JobProperties Job { get; set; }

    /// <summary>
    /// Gets or sets the player death level.
    /// </summary>
    public int DeathLevel { get; set; }

    /// <summary>
    /// Gets or sets the player mode.
    /// </summary>
    public ModeType Mode { get; set; }

    /// <summary>
    /// Gets the player's appearance.
    /// </summary>
    public HumanVisualAppearance Appearence { get; init; }

    /// <summary>
    /// Gets or sets the player's inventory.
    /// </summary>
    public Inventory Inventory { get; }

    /// <summary>
    /// Gets or sets the player's available statistic points.
    /// </summary>
    public int AvailablePoints { get; set; }

    /// <summary>
    /// Gets or sets the player's available skill points.
    /// </summary>
    public ushort SkillPoints { get; set; }

    /// <summary>
    /// Gets the player's gold.
    /// </summary>
    public Gold Gold { get; }

    /// <summary>
    /// Gets the player's experience.
    /// </summary>
    public Experience Experience { get; }

    /// <summary>
    /// Gets the player's skills.
    /// </summary>
    public SkillTree Skills { get; }

    /// <summary>
    /// Gets the player's quest diary.
    /// </summary>
    public QuestDiary QuestDiary { get; }

    /// <summary>
    /// Gets or sets the current shop name that the player is visiting.
    /// </summary>
    public string CurrentShopName { get; set; }

    /// <summary>
    /// Gets the player taskbar.
    /// </summary>
    public Taskbar Taskbar { get; }

    public Player(FFUserConnection connection, MoverProperties properties)
        : base(properties)
    {
        _connection = connection;
        Inventory = new Inventory(owner: this);
        Gold = new Gold(this);
        Experience = new Experience(this);
        Skills = new SkillTree(this);
        QuestDiary = new QuestDiary(this);
        Taskbar = new Taskbar();
    }

    /// <summary>
    /// Update the player's logic.
    /// </summary>
    public void Update()
    {
        if (IsDead || !IsSpawned)
        {
            return;
        }

        if (!IsFighting)
        {
            Health.IdleHeal();
        }

        LookAround();
        UpdateMoves();
    }

    /// <summary>
    /// Looks around for other entities.
    /// </summary>
    public void LookAround()
    {
        if (!IsSpawned || !IsVisible)
        {
            return;
        }

        IEnumerable<WorldObject> currentVisibleEntities = MapLayer.GetVisibleObjects(this).ToList();
        IEnumerable<WorldObject> appearingEntities = currentVisibleEntities.Except(VisibleObjects).ToList();
        IEnumerable<WorldObject> disapearingEntities = VisibleObjects.Except(currentVisibleEntities).ToList();

        if (appearingEntities.Any() || disapearingEntities.Any())
        {
            FFSnapshot snapshot = new();

            foreach (WorldObject appearingObject in appearingEntities)
            {
                snapshot.Merge(new AddObjectSnapshot(appearingObject, AddObjectSnapshot.PlayerAddObjMethodType.ExcludeItems));

                if (appearingObject is Mover appearingMover && appearingMover.IsMoving)
                {
                    snapshot.Merge(new DestPositionSnapshot(appearingMover));
                }

                AddVisibleEntity(appearingObject);
            }

            foreach (WorldObject disapearingObject in disapearingEntities)
            {
                snapshot.Merge(new DeleteObjectSnapshot(disapearingObject));

                RemoveVisibleEntity(disapearingObject);
            }

            Send(snapshot);
        }
    }

    /// <summary>
    /// Gets the equiped items.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Item> GetEquipedItems() => Inventory.GetRange(Inventory.InventorySize, Inventory.InventoryEquipParts).Select(x => x.Item);

    /// <summary>
    /// Updates the player's statistics.
    /// </summary>
    /// <param name="strength"></param>
    /// <param name="stamina"></param>
    /// <param name="dexterity"></param>
    /// <param name="intelligence"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void UpdateStatistics(int strength, int stamina, int dexterity, int intelligence)
    {
        int total = strength + stamina + dexterity + intelligence;

        if (AvailablePoints <= 0 || total > AvailablePoints)
        {
            throw new InvalidOperationException($"{Name} doesn't have enough statistic points.");
        }

        if (strength > AvailablePoints || stamina > AvailablePoints ||
            dexterity > AvailablePoints || intelligence > AvailablePoints || total <= 0 ||
            total > ushort.MaxValue)
        {
            throw new InvalidOperationException("Statistics point bad calculation. (Hack attempt)");
        }

        Statistics.Strength += strength;
        Statistics.Stamina += stamina;
        Statistics.Dexterity += dexterity;
        Statistics.Intelligence += intelligence;
        AvailablePoints -= (ushort)total;

        Health.RegenerateAll();
        Defense.Update();

        using SetStatisticsStateSnapshot setStateSnapshot = new(this);
        Send(setStateSnapshot);
    }

    /// <summary>
    /// Resets the player's statistics.
    /// </summary>
    public void ResetStatistics()
    {
        DefaultCharacterOptions defaultCharacter = Appearence.Gender == GenderType.Male ?
            GameOptions.Current.DefaultCharacter.Man :
            GameOptions.Current.DefaultCharacter.Woman;

        Statistics.Strength = defaultCharacter.Strength;
        Statistics.Stamina = defaultCharacter.Stamina;
        Statistics.Dexterity = defaultCharacter.Dexterity;
        Statistics.Intelligence = defaultCharacter.Intelligence;
        AvailablePoints = (ushort)((Level - 1) * 2);

        Health.RegenerateAll();
        Defense.Update();

        using SetStatisticsStateSnapshot setStateSnapshot = new(this);
        Send(setStateSnapshot);
    }

    /// <summary>
    /// Adds the given amount of skill points to the current player.
    /// </summary>
    /// <param name="skillPointsToAdd">Skill point amount to add.</param>
    /// <param name="sendToPlayer">Boolean value that indicates if the packet should be sent to the player.</param>
    public void AddSkillPoints(ushort skillPointsToAdd, bool sendToPlayer = true)
    {
        SkillPoints += skillPointsToAdd;

        if (sendToPlayer)
        {
            using SetExperienceSnapshot snapshot = new(this);
            Send(snapshot);
        }
    }

    /// <summary>
    /// Resets the player skill levels.
    /// </summary>
    public void ResetSkills()
    {
        foreach (Skill skill in Skills)
        {
            SkillPoints += (ushort)(skill.Level * SkillTree.SkillPointUsage[skill.Properties.JobType]);
            skill.Level = 0;
        }
    }

    /// <summary>
    /// Resets the avaialble skill points.
    /// </summary>
    public void ResetAvailableSkillPoints()
    {
        SkillPoints = 0;
    }

    /// <summary>
    /// Sends an oral text message to every entities around.
    /// </summary>
    /// <param name="message">Message.</param>
    public void Speak(string message)
    {
        using ChatSnapshot snapshot = new(this, message);

        SendToVisible(snapshot, sendToSelf: true);
    }

    /// <summary>
    /// Picks up an item from the map.
    /// </summary>
    /// <param name="mapItem">Map item to pickup.</param>
    /// <param name="sendPickupMotion">Boolean value that indicates if the player should play a pickup motion.</param>
    public void PickupItem(MapItemObject mapItem, bool sendPickupMotion = true)
    {
        if (mapItem.HasOwner && mapItem.Owner != this)
        {
            SendDefinedText(DefineText.TID_GAME_PRIORITYITEMPER, $"\"{mapItem.Item.Name}\"");
            return;
        }

        bool itemPickedUp;

        if (mapItem.IsGold)
        {
            itemPickedUp = Gold.Increase(mapItem.Item.Quantity);
        }
        else
        {
            itemPickedUp = Inventory.CreateItem(mapItem.Item) > 0;
            SendDefinedText(DefineText.TID_GAME_REAPITEM, $"\"{mapItem.Item.Name}\"");
        }

        if (itemPickedUp)
        {
            if (mapItem.ItemType == MapItemType.QuestItem)
            {
                mapItem.Despawn();
            }
            else
            {
                MapLayer.RemoveItem(mapItem);
            }
        }

        if (sendPickupMotion)
        {
            using MotionSnapshot motionSnapshot = new(this, ObjectMessageType.OBJMSG_PICKUP);
            SendToVisible(motionSnapshot, sendToSelf: true);
        }
    }

    /// <summary>
    /// Teleports the player to the given map and position.
    /// </summary>
    /// <param name="mapId">Map Id.</param>
    /// <param name="position">Destination position.</param>
    /// <param name="sendToPlayer">Boolean value that indicates if the information should be sent to the player.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Teleport(int mapId, Vector3 position, bool sendToPlayer = true)
    {
        void SetPlayerPosition(Vector3 newPosition)
        {
            Unfollow();
            StopMoving();
            Position.Copy(newPosition);
        }

        if (Map.Id == mapId)
        {
            if (!Map.IsInBounds(position))
            {
                throw new InvalidOperationException($"Attempt to teleport '{Name}' to an invalid position: {position} in map: '{Map.Name}'.");
            }

            SetPlayerPosition(position);

            using FFSnapshot snapshots = new(new FFSnapshot[]
            {
                new SetPositionSnapshot(this),
                new WorldReadInfoSnapshot(this)
            });
            SendToVisible(snapshots, sendToPlayer);
        }
        else
        {
            Map destinationMap = MapManager.Current.Get(mapId) ?? throw new InvalidOperationException($"Cannot teleport to map with id: '{mapId}'. Map not found.");

            if (!destinationMap.IsInBounds(position))
            {
                throw new InvalidOperationException($"Attempt to teleport '{Name}' to an invalid position: {position} in map: '{destinationMap.Name}'.");
            }

            IsSpawned = false;
            MapLayer.RemovePlayer(this);

            SetPlayerPosition(position);

            Map = destinationMap;
            MapLayer = destinationMap.GetDefaultLayer();
            MapLayer.AddPlayer(this);

            if (sendToPlayer)
            {
                using FFSnapshot snapshots = new(new FFSnapshot[]
                {
                    new ReplaceSnapshot(this),
                    new WorldReadInfoSnapshot(this),
                    new AddObjectSnapshot(this)
                });

                Send(snapshots);
            }

            IsSpawned = true;
        }
    }

    public override void OnTargetKilled(Mover target)
    {
        if (target is Player)
        {
            // TODO: PK
        }
        else if (target is Monster monster)
        {
            Experience.Increase(monster.Properties.Experience * GameOptions.Current.Rates.Experience);
            QuestDiary.OnMonsterKilled(monster);
        }
    }

    public override void OnKilled(Mover killer)
    {
        base.OnKilled(killer);
    }

    public override void Dispose()
    {
        foreach (WorldObject visibleObject in VisibleObjects)
        {
            if (visibleObject is not Player)
            {
                visibleObject.VisibleObjects.Remove(this);
            }
        }

        MapLayer.RemovePlayer(this);
    }

    protected override void OnArrived()
    {
        if (IsFollowing && FollowTarget is MapItemObject mapItem)
        {
            PickupItem(mapItem);
            Unfollow();
        }
    }

    /// <summary>
    /// Sends a motion to every entities around.
    /// </summary>
    /// <param name="motion">motionId.</param>
    public void Motion(ObjectMessageType motionEnum)
    {
        using MotionSnapshot snapshot = new(this, motionEnum);

        SendToVisible(snapshot, sendToSelf: true);
    }

    /// <summary>
    /// Cancels the skill usage.
    /// </summary>
    public void CancelSkillUsage()
    {
        using ClearUseSkillSnapshot snapshot = new(this);

        SendToVisible(snapshot, sendToSelf: true);
    }

    /// <summary>
    /// Sends a packet to the player.
    /// </summary>
    /// <param name="packet">Packet to send.</param>
    public override void Send(FFPacket packet) => _connection.Send(packet);

    private void AddVisibleEntity(WorldObject entity)
    {
        if (!VisibleObjects.Contains(entity))
        {
            VisibleObjects.Add(entity);
        }

        if (entity is not Player && !entity.VisibleObjects.Contains(this))
        {
            entity.VisibleObjects.Add(this);
        }
    }

    private void RemoveVisibleEntity(WorldObject entity)
    {
        if (VisibleObjects.Contains(entity))
        {
            VisibleObjects.Remove(entity);
        }

        if (entity.VisibleObjects.Contains(this))
        {
            entity.VisibleObjects.Remove(this);
        }
    }
}
