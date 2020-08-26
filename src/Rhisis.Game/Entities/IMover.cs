using Rhisis.Core.Structures.Game;

namespace Rhisis.Game.Entities
{
    public interface IMover
    {
        int Level { get; set; }

        Point3D DestinationPosition { get; set; }

        float Speed { get; }

        float SpeedFactor { get; set; }

        bool IsMoving { get; }

        MoverData Data { get; }
    }
}
