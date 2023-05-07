using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Game.Common;
using Rhisis.Game.Factories;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Rhisis.Game.Entities;

public sealed class Monster : Mover
{
    private long _nextMoveTime;
    private long _nextAttackTime;
    private long _nextRespawnTime;
    private long _despawnTime;
    public bool _isReturningToBeginPosition;

    public int RespawnTime { get; init; }

    public Rectangle Region { get; init; }

    public Vector3 BeginPosition { get; init; }

    public Monster(MoverProperties properties) 
        : base(properties)
    {
        Name = properties.Name;
    }

    public void Update()
    {
        if (IsDead)
        {
            if (CanDespawn())
            {
                Despawn();
            }
            else if (CanRespawn())
            {
                Respawn();
            }

            return;
        }

        if (IsFighting && Target is not null)
        {
            if (IsFollowing)
            {
                if (SpeedFactor != 2)
                {
                    SetSpeedFactor(2);
                }

                if (Position.IsInCircle(FollowTarget.Position, FollowDistance))
                {
                    if (_nextAttackTime < Time.TimeInMilliseconds())
                    {
                        TryMeleeAttack(Target, AttackType.MeleeAttack1);
                        _nextAttackTime = (long)(Time.TimeInMilliseconds() + Properties.ReAttackDelay);
                    }
                }
                else
                {
                    if (Position.IsInRange(BeginPosition, 40f))
                    {
                        Follow(Target);
                    }
                    else
                    {
                        ReturnToBeginPosition();
                    }
                }
            }
            else
            {
                Follow(Target);
            }
        }
        else
        {
            if (ObjectState.HasFlag(ObjectState.OBJSTA_STAND) && _nextMoveTime < Time.TimeInSeconds())
            {
                Vector3 randomPosition = Region.GetRandomPosition();

                while (Position.GetDistance2D(randomPosition) > 10f)
                {
                    randomPosition = Region.GetRandomPosition();
                }

                if (Properties.IsFlying)
                {
                    randomPosition.Y = Map.GetHeight(randomPosition.X, randomPosition.Z) + FFRandom.Random(0, 6);
                }

                Move(randomPosition.X, randomPosition.Y, randomPosition.Z);
            }

            if (ObjectState.HasFlag(ObjectState.OBJSTA_FMOVE))
            {
                if (_isReturningToBeginPosition && Position.IsInCircle(BeginPosition, 3.0f) && SpeedFactor >= 2)
                {
                    SetSpeedFactor(1f);
                }
            }
        }

        UpdateMoves();
    }

    protected override void OnArrived()
    {
        if (!IsFighting)
        {
            long nextMoveTime = _isReturningToBeginPosition ? 
                FFRandom.LongRandom(1, 3) :
                FFRandom.LongRandom(5, 10);

            _nextMoveTime = Time.TimeInSeconds() + nextMoveTime;
            BeginPosition.Copy(Position);

            if (_isReturningToBeginPosition)
            {
                Health.RegenerateAll();
                _isReturningToBeginPosition = false;
            }

            if (SpeedFactor > 2)
            {
                SetSpeedFactor(1);
            }
        }

    }

    protected override void OnSufferDamages(Mover attacker, int damages, AttackFlags attackFlags)
    {
        if (IsDead)
        {
            Unfollow();
            Target = null;
            IsFighting = false;
        }
        else
        {
            Follow(attacker);
            Target = attacker;
            IsFighting = true;
        }
    }

    public override void OnKilled(Mover killer)
    {
        Console.WriteLine($"Killed by {killer.Name}...:(");

        DropGold(killer);
        DropItems(killer);
        IsFighting = false;
        Target = null;
        _despawnTime = Time.TimeInSeconds() + 3;
    }

    public override void OnTargetKilled(Mover target)
    {
        ReturnToBeginPosition();
    }

    private void SetSpeedFactor(float speedFactor)
    {
        SpeedFactor = speedFactor;

        using SetSpeedFactorSnapshot snapshot = new(this, speedFactor);
        SendToVisible(snapshot);
    }

    private void ReturnToBeginPosition()
    {
        _isReturningToBeginPosition = true;
        Unfollow();
        Target = null;
        SetSpeedFactor(2.66f);
        Move(BeginPosition.X, BeginPosition.Y, BeginPosition.Z);
    }

    private bool CanDespawn()
    {
        return IsSpawned && IsDead && _despawnTime < Time.TimeInSeconds();
    }

    private void Despawn()
    {
        IsSpawned = false;
        _nextRespawnTime = Time.TimeInSeconds() + RespawnTime;
    }

    private bool CanRespawn()
    {
        return !IsSpawned && IsDead && _nextRespawnTime < Time.TimeInSeconds();
    }

    private void Respawn()
    {
        Position.Copy(Region.GetRandomPosition());
        DestinationPosition.Reset();
        Health.RegenerateAll();
        Target = null;
        Unfollow();
        ObjectState = ObjectState.OBJSTA_STAND;
        _nextMoveTime = Time.TimeInSeconds() + FFRandom.LongRandom(5, 15);
        SpeedFactor = 1;
        IsSpawned = true;
    }

    private void DropGold(Mover owner)
    {
        const int DropGoldLimit1 = 9;
        const int DropGoldLimit2 = 49;
        const int DropGoldLimit3 = 99;
        int goldMultiplier = GameOptions.Current.Rates.Gold;
        int goldAmount = Math.Max(0, FFRandom.Random(Properties.DropGoldMin, Properties.DropGoldMax)) * goldMultiplier;

        if (goldAmount > 0)
        {
            int goldItemId = goldAmount switch
            {
                int amount when amount > DropGoldLimit1 * goldMultiplier => DefineItem.II_GOLD_SEED2,
                int amount when amount > DropGoldLimit2 * goldMultiplier => DefineItem.II_GOLD_SEED3,
                int amount when amount > DropGoldLimit3 * goldMultiplier => DefineItem.II_GOLD_SEED4,
                _ => DefineItem.II_GOLD_SEED1
            };

            Item goldItem = new(GameResources.Current.Items.Get(goldItemId))
            {
                Quantity = goldAmount
            };

            DropItem(goldItem, owner);
        }
    }

    private void DropItems(Mover owner)
    {
        // TODO: move this constant to configuration file
        const long MaxDropChance = 3000000000;

        // Drop items
        int itemCount = 0;
        foreach (DropItemProperties dropItemProperties in Properties.DropItems)
        {
            if (itemCount >= Properties.MaxDropItem)
                break;

            long dropChance = FFRandom.LongRandom(0, MaxDropChance);

            if (dropItemProperties.Probability * GameOptions.Current.Rates.Drop >= dropChance)
            {
                byte itemRefine = (byte)FFRandom.Random(0, dropItemProperties.ItemMaxRefine);
                ItemProperties itemProperties = GameResources.Current.Items.Get(dropItemProperties.ItemId);
                Item itemToDrop = new(itemProperties)
                {
                    Refine = (byte)itemRefine,
                    Quantity = 1
                };

                DropItem(itemToDrop, owner);

                itemCount++;
            }
        }

        // Drop item kinds
        foreach (DropItemKindProperties dropItemKind in Properties.DropItemsKind)
        {
            IEnumerable<ItemProperties> itemsPropertiesByItemKind = GameResources.Current.Items
                .Where(x => x.ItemKind3 == dropItemKind.ItemKind && x.Rare >= dropItemKind.UniqueMin && x.Rare <= dropItemKind.UniqueMax);

            if (!itemsPropertiesByItemKind.Any())
            {
                continue;
            }

            ItemProperties itemProperties = itemsPropertiesByItemKind.ElementAt(FFRandom.Random(0, itemsPropertiesByItemKind.Count() - 1));
            int itemRefine = FFRandom.Random(0, 10);

            for (int i = itemRefine; i >= 0; i--)
            {
                long itemDropProbability = (long)(GameResources.Current.ExperienceTable.GetDropLuck(itemProperties.Level > 120 ? 119 : itemProperties.Level, itemRefine) * (Properties.CorrectionValue / 100f));
                long dropChance = FFRandom.LongRandom(0, MaxDropChance);

                if (dropChance < itemDropProbability * GameOptions.Current.Rates.Drop)
                {
                    Item itemToDrop = new(itemProperties)
                    {
                        Refine = (byte)itemRefine,
                        Quantity = 1
                    };

                    DropItem(itemToDrop, owner);

                    break;
                }
            }
        }
    }
}