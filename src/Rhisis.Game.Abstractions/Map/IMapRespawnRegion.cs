using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMapRespawnRegion : IMapRegion
    {
        /// <summary>
        /// Gets the respawn region object type.
        /// </summary>
        WorldObjectType ObjectType { get; }

        /// <summary>
        /// Gets the mover model id of the current respawn region.
        /// </summary>
        int ModelId { get; }

        /// <summary>
        /// Gets the repawn time of this region.
        /// </summary>
        int Time { get; }

        /// <summary>
        /// Gets the number of movers to spawn on this region.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the respawn basic height.
        /// </summary>
        float Height { get; }
    }
}
