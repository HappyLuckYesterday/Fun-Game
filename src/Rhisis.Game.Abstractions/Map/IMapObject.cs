using Rhisis.Core.Structures;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMapObject
    {
        int ModelId { get; }

        Vector3 Position { get; }

        float Angle { get; }
    }
}
