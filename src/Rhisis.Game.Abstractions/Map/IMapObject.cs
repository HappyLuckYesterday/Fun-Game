using Rhisis.Core.Structures;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMapObject
    {
        /// <summary>
        /// Gets the map object model id.
        /// </summary>
        int ModelId { get; }

        /// <summary>
        /// Gets the map object position.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Gets the map object orientation angle.
        /// </summary>
        float Angle { get; }
    }
}
