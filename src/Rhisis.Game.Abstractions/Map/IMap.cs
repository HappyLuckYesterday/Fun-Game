using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMap
    {
        int Id { get; }

        string Name { get; }

        int Width { get; }

        int Length { get; }

        int RevivalMapId { get; }

        IEnumerable<IMapLayer> Layers { get; }

        IEnumerable<IMapRegion> Regions { get; }

        IMapLayer GenerateNewLayer();

        void Process();

        float GetHeight(float positionX, float positionZ);
    }
}
