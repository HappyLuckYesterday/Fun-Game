using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common.Resources;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IMover : IWorldObject, IInteligentEntity
    {
        int Level { get; set; }

        Vector3 DestinationPosition { get; set; }

        float Speed { get; }

        float SpeedFactor { get; set; }

        bool IsMoving { get; }

        MoverData Data { get; }

        IHealth Health { get; }

        IDefense Defense { get; }

        IStatistics Statistics { get; }

        IAttributes Attributes { get; }

        IWorldObject FollowTarget { get; set; }

        float FollowDistance { get; set; }

        bool IsFollowing => FollowTarget != null;

        void Follow(IWorldObject worldObject);

        void Unfollow();
    }
}
