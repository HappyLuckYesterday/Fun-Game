using Rhisis.Core.Structures;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMap : IDisposable
    {
        int Id { get; }

        string Name { get; }

        int Width { get; }

        int Length { get; }

        int RevivalMapId { get; }

        IMapRevivalRegion DefaultRevivalRegion { get; }

        IEnumerable<IMapLayer> Layers { get; }

        IEnumerable<IMapRegion> Regions { get; }

        IEnumerable<IMapObject> Objects { get; }

        IMapLayer GenerateNewLayer();

        IMapLayer GetMapLayer(int layerId);

        IMapLayer GetDefaultMapLayer();

        float GetHeight(float positionX, float positionZ);

        void StartUpdate();

        void StopUpdate();

        bool IsInBounds(float x, float y, float z);

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
