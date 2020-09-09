using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Game;
using Rhisis.Game.Abstractions.Components;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IMover : IWorldObject
    {
        int Level { get; set; }

        Vector3 DestinationPosition { get; set; }

        float Speed { get; }

        float SpeedFactor { get; set; }

        bool IsMoving { get; }

        MoverData Data { get; }

        IHealth Health { get; }
    }
}
