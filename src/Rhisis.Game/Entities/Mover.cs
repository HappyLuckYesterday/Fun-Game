using Rhisis.Core.IO;
using Rhisis.Game.Battle;
using Rhisis.Game.Battle.AttackArbiters;
using Rhisis.Game.Battle.AttackArbiters.Reducers;
using Rhisis.Game.Common;
using Rhisis.Game.Extensions;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots.Battle;
using Rhisis.Game.Resources.Properties;
using System;

namespace Rhisis.Game.Entities;

public class Mover : WorldObject
{
    public override WorldObjectType Type => WorldObjectType.Mover;

    /// <summary>
    /// Gets the mover speed.
    /// </summary>
    public virtual float Speed => (Properties.Speed + (Attributes.Get(DefineAttributes.DST_SPEED) / 100)) * SpeedFactor;

    /// <summary>
    /// Gets or sets the mover speed factor.
    /// </summary>
    public float SpeedFactor { get; set; } = 1;

    /// <summary>
    /// Gets or sets the mover destination position.
    /// </summary>
    public Vector3 DestinationPosition { get; set; } = new();

    /// <summary>
    /// Gets the mover properties.
    /// </summary>
    public MoverProperties Properties { get; }

    /// <summary>
    /// Gets or sets the mover level.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Gets a boolean value that indicates if the mover is dead.
    /// </summary>
    public bool IsDead => Health.Hp <= 0;

    /// <summary>
    /// Gets a boolean value that indicates if the mover is moving.
    /// </summary>
    public bool IsMoving => (ObjectState & ObjectState.OBJSTA_MOVE_ALL) != 0 && !DestinationPosition.IsZero();

    /// <summary>
    /// Gets or sets a boolean value that indicates if the mover is in a fight.
    /// </summary>
    public bool IsFighting { get; set; }

    /// <summary>
    /// Gets the mover health.
    /// </summary>
    public Health Health { get; init; }

    /// <summary>
    /// Gets the attributes.
    /// </summary>
    public Attributes Attributes { get; }

    /// <summary>
    /// Gets the player's statistics.
    /// </summary>
    public Statistics Statistics { get; init; }

    /// <summary>
    /// Gets or sets the follow target.
    /// </summary>
    public WorldObject FollowTarget { get; set; }

    /// <summary>
    /// Gets a boolean value that indicates if the mover is following another mover.
    /// </summary>
    public bool IsFollowing => FollowTarget is not null;

    /// <summary>
    /// Gets or sets the follow distance.
    /// </summary>
    public float FollowDistance { get; set; }

    /// <summary>
    /// Gets or sets the mover's actual target.
    /// </summary>
    public Mover Target { get; set; }

    /// <summary>
    /// Gets the mover's defense.
    /// </summary>
    public Defense Defense { get; }

    protected Mover(MoverProperties properties)
    {
        Properties = properties ?? throw new ArgumentNullException(nameof(properties), "Cannot create a mover with no properties.");
        Attributes = new Attributes(this);
        Statistics = new Statistics(this);
        Health = new Health(this);
        Defense = new Defense(this);
    }
    
    /// <summary>
    /// Indicates that the mover should move to the given position.
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    public void Move(float x, float y, float z)
    {
        ObjectState |= ObjectState.OBJSTA_FMOVE;
        ObjectState &= ~ObjectState.OBJSTA_STAND;
        DestinationPosition = new(x, y, z);
        RotationAngle = Vector3.AngleBetween(Position, DestinationPosition);

        using DestPositionSnapshot packet = new(this);
        SendToVisible(packet);
    }

    /// <summary>
    /// Stops moving the current entity.
    /// </summary>
    public void StopMoving()
    {
        ObjectState &= ~ObjectState.OBJSTA_FMOVE;
        ObjectState |= ObjectState.OBJSTA_STAND;

        DestinationPosition.Reset();
        OnArrived();
    }

    /// <summary>
    /// Follows a target.
    /// </summary>
    /// <param name="target">Target to follow.</param>
    /// <param name="distance">Follow distance.</param>
    public void Follow(WorldObject target, float distance = 1f)
    {
        FollowTarget = target;
        FollowDistance = distance;
        DestinationPosition.Copy(target.Position);
        ObjectState &= ~ObjectState.OBJSTA_STAND;
        ObjectState |= ObjectState.OBJSTA_FMOVE;

        using MoverSetDestObjectSnapshot snapshot = new(this, target, distance);
        SendToVisible(snapshot);
    }

    /// <summary>
    /// Unfollow a target.
    /// </summary>
    public void Unfollow()
    {
        FollowTarget = null;
        FollowDistance = 0;
    }

    /// <summary>
    /// Drops the given item to the ground.
    /// </summary>
    /// <param name="item">Item to drop.</param>
    /// <param name="owner">Item owner.</param>
    public void DropItem(Item item, Mover owner = null)
    {
        MapItemObject itemObject = new(item)
        {
            Position = Position.Clone(),
            IsSpawned = true,
            IsVisible = true,
            Map = Map,
            MapLayer = MapLayer,
            Owner = owner,
            OwnershipTime = owner is not null ? Time.TimeInSeconds() + GameOptions.Current.Drops.OwnershipTime : 0
        };

        MapLayer.AddItem(itemObject);
    }

    /// <summary>
    /// Check if the current mover can attack the given target.
    /// </summary>
    /// <param name="target">Target to attack.</param>
    /// <returns>True if can attack; false otherwise.</returns>
    public bool CanAttack(Mover target)
    {
        if (IsDead)
        {
            return false;
        }

        if (target == this)
        {
            return false;
        }

        if (target.IsDead)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Tries to inflect a melee attack to the given target.
    /// </summary>
    /// <param name="target">Target to attack.</param>
    /// <param name="attackType">Attack type.</param>
    /// <returns>True if the melee attack succeded; false othewise.</returns>
    public bool TryMeleeAttack(Mover target, AttackType attackType)
    {
        if (!CanAttack(target) || !attackType.IsMeleeAttack())
        {
            return false;
        }

        Target = target;

        if (!TryInflictDamagesIfOneHitKillMode(target, attackType, out AttackResult attackResult))
        {
            attackResult = new MeleeAttackArbiter(this, target).CalculateDamages();

            if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
            {
                attackResult = new MeleeAttackReducer(this, target).ReduceDamages(attackResult);

                InflictDamages(target, attackResult, attackType);
            }
        }

        using MeleeAttackSnapshot meleeAttackSnapshot = new(this, target, attackType, attackResult.Flags);
        SendToVisible(meleeAttackSnapshot);
        
        return true;
    }

    private void InflictDamages(Mover target, AttackResult attackResult, AttackType attackType)
    {
        Target = target;
        target.Health.SufferDamages(this, Math.Max(0, attackResult.Damages), attackType, attackResult.Flags);
        target.OnSufferDamages(this, attackResult.Damages, attackResult.Flags);
    }

    private bool TryInflictDamagesIfOneHitKillMode(Mover target, AttackType attackType, out AttackResult attackResult)
    {
        if (this is Player player && player.Mode.HasFlag(ModeType.ONEKILL_MODE))
        {
            attackResult = new AttackResult()
            {
                Damages = target.Health.Hp,
                Flags = AttackFlags.AF_GENERIC
            };

            InflictDamages(target, attackResult, attackType);
            return true;
        }

        attackResult = null;
        return false;
    }

    /// <summary>
    /// Updates
    /// </summary>
    protected void UpdateMoves()
    {
        if (!IsMoving)
        {
            return;
        }

        float arrivalRange = IsFollowing ? FollowDistance : 1f;

        if (Position.IsInCircle(DestinationPosition, arrivalRange))
        {
            Position.Copy(DestinationPosition);
            StopMoving();
        }
        else
        {
            float entitySpeed = Speed;

            if (ObjectStateFlags.HasFlag(StateFlags.OBJSTAF_WALK))
            {
                entitySpeed /= 4f;
            }
            else if (ObjectState.HasFlag(ObjectState.OBJSTA_BMOVE))
            {
                entitySpeed /= 5f;
            }

            float distanceX = DestinationPosition.X - Position.X;
            float distanceZ = DestinationPosition.Z - Position.Z;
            double distance = Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);

            // Normalize
            double offsetX = distanceX / distance * entitySpeed;
            double offsetZ = distanceZ / distance * entitySpeed;

            if (Math.Abs(offsetX) > Math.Abs(distanceX))
            {
                offsetX = distanceX;
            }

            if (Math.Abs(offsetZ) > Math.Abs(distanceZ))
            {
                offsetZ = distanceZ;
            }

            Position.X += (float)offsetX;
            Position.Z += (float)offsetZ;
        }
    }

    protected virtual void OnArrived()
    {
    }

    protected virtual void OnSufferDamages(Mover attacker, int damages, AttackFlags attackFlags)
    {
    }

    /// <summary>
    /// Triggered when the current mover has been killed.
    /// </summary>
    /// <param name="killer">Killer.</param>
    public virtual void OnKilled(Mover killer)
    {
    }

    /// <summary>
    /// Triggered when the current mover has killed the given target.
    /// </summary>
    /// <param name="target">Killed target.</param>
    public virtual void OnTargetKilled(Mover target)
    {
    }
}
