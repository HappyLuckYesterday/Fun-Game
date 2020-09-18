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

        IEnumerable<IMapLayer> Layers { get; }

        IEnumerable<IMapRegion> Regions { get; }

        IMapLayer GenerateNewLayer();

        IMapLayer GetMapLayer(int layerId);

        float GetHeight(float positionX, float positionZ);

        void StartUpdate();

        void StopUpdate();
    }
}
