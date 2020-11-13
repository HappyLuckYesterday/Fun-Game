using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common.Resources;

namespace Rhisis.Game.Abstractions.Entities
{
    /// <summary>
    /// Describes the mover entity.
    /// </summary>
    public interface IMover : IWorldObject, IInteligentEntity
    {
        /// <summary>
        /// Gets or sets the mover level.
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Gets or sets the mover destination position.
        /// </summary>
        Vector3 DestinationPosition { get; set; }

        /// <summary>
        /// Gets the mover speed.
        /// </summary>
        float Speed { get; }

        /// <summary>
        /// Gets or sets the mover speed factor.
        /// </summary>
        float SpeedFactor { get; set; }

        /// <summary>
        /// Gets a boolean value that indicates if the mover is currently moving.
        /// </summary>
        bool IsMoving { get; }

        /// <summary>
        /// Gets the mover data.
        /// </summary>
        MoverData Data { get; }

        /// <summary>
        /// Gets the mover health.
        /// </summary>
        IHealth Health { get; }

        /// <summary>
        /// Gets the mover defense.
        /// </summary>
        IDefense Defense { get; }

        /// <summary>
        /// Gets the mover statistics.
        /// </summary>
        IStatistics Statistics { get; }

        /// <summary>
        /// Gets the mover attributes.
        /// </summary>
        IAttributes Attributes { get; }

        /// <summary>
        /// Gets the mover projectiles in use.
        /// </summary>
        IProjectiles Projectiles { get; }

        /// <summary>
        /// Gets the mover action delayer.
        /// </summary>
        IDelayer Delayer { get; }

        /// <summary>
        /// Gets the mover active buffs.
        /// </summary>
        IBuffs Buffs { get; }

        /// <summary>
        /// Gets the mover follow target.
        /// </summary>
        IWorldObject FollowTarget { get; set; }

        /// <summary>
        /// Gets the mover follow distance.
        /// </summary>
        float FollowDistance { get; set; }

        /// <summary>
        /// Gets a boolean value that indicates if the mover is currently following some other object.
        /// </summary>
        bool IsFollowing => FollowTarget != null;

        /// <summary>
        /// Follows the given object.
        /// </summary>
        /// <param name="worldObject">Object to follow.</param>
        void Follow(IWorldObject worldObject);

        /// <summary>
        /// Stops following the current follow target.
        /// </summary>
        void Unfollow();
    }
}
