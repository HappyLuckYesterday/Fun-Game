using Rhisis.Core.Structures;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMap : IDisposable
    {
        /// <summary>
        /// Gets the map id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the map name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the map width in number of lands.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the map length in number of lands.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets the revival map id when a player dies on this map.
        /// </summary>
        int RevivalMapId { get; }

        /// <summary>
        /// Gets the default revival region when a player dies on this map.
        /// </summary>
        IMapRevivalRegion DefaultRevivalRegion { get; }

        /// <summary>
        /// Gets the current layers list of this map.
        /// </summary>
        IEnumerable<IMapLayer> Layers { get; }

        /// <summary>
        /// Gets the map regions.
        /// </summary>
        IEnumerable<IMapRegion> Regions { get; }

        /// <summary>
        /// Gets the map objects.
        /// </summary>
        IEnumerable<IMapObject> Objects { get; }

        /// <summary>
        /// Generates a new map layer by instancing all static objects and monsters.
        /// </summary>
        /// <returns></returns>
        IMapLayer GenerateNewLayer();

        /// <summary>
        /// Gets a map layer by its id.
        /// </summary>
        /// <param name="layerId">Layer id.</param>
        /// <returns>The layer if found; the default map layer otherwise.</returns>
        IMapLayer GetMapLayer(int layerId);

        /// <summary>
        /// Gets the default map layer.
        /// </summary>
        /// <returns>The default map layer.</returns>
        IMapLayer GetDefaultMapLayer();

        /// <summary>
        /// Gets the map height at a given X and Z position.
        /// </summary>
        /// <param name="positionX">X coordinate.</param>
        /// <param name="positionZ">Z coordinate.</param>
        /// <returns>Height.</returns>
        float GetHeight(float positionX, float positionZ);

        /// <summary>
        /// Starts updating the map entities.
        /// </summary>
        void StartUpdate();

        /// <summary>
        /// Stops updating the map entities.
        /// </summary>
        void StopUpdate();

        /// <summary>
        /// Check if the given X,Y,Z coordinates are in map bounds.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        /// <returns>True if the position is in map bounds; false otherwise.</returns>
        bool IsInBounds(float x, float y, float z);

        /// <summary>
        /// Check if the given position is in map bounds.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <returns>True if the position is in map bounds; false otherwise.</returns>
        bool IsInBounds(Vector3 position);

        /// <summary>
        /// Gets the nearest revival region from a given position.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <returns>The nearest revival region.</returns>
        IMapRevivalRegion GetNearRevivalRegion(Vector3 position);

        /// <summary>
        /// Gets the nearest revival region from a given position and if the region should be for chao mode (PK mode).
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="isChaoMode">Region is chao mode (PK mode).</param>
        /// <returns>The nearest revival region.</returns>
        IMapRevivalRegion GetNearRevivalRegion(Vector3 position, bool isChaoMode);

        /// <summary>
        /// Gets a revival region by his key.
        /// </summary>
        /// <param name="revivalKey">Revival region key.</param>
        /// <returns>Revival region matching the key.</returns>
        IMapRevivalRegion GetRevivalRegion(string revivalKey);

        /// <summary>
        /// Gets a revival region by his key and if the region should be for chao mode (PK mode).
        /// </summary>
        /// <param name="revivalKey">Revival region key.</param>
        /// <param name="isChaoMode">Region is chao mode (PK mode).</param>
        /// <returns>Revival region matching the key and the chao mode.</returns>
        IMapRevivalRegion GetRevivalRegion(string revivalKey, bool isChaoMode);
    }
}
