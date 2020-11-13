using Rhisis.Game.Abstractions.Resources;

namespace Rhisis.Game.Abstractions.Map
{
    /// <summary>
    /// Provides a mechanism to manage the maps.
    /// </summary>
    public interface IMapManager : IGameResourceLoader
    {
        /// <summary>
        /// Gets a map by its id.
        /// </summary>
        /// <param name="mapId">Map Id.</param>
        /// <returns>The map if found; <see cref="null"/> otherwise.</returns>
        IMap GetMap(int mapId);
    }
}
