using System.Collections.Generic;

namespace Rhisis.Game.Abstractions
{
    public interface IMap
    {
        int Id { get; }

        int Width { get; }

        int Length { get; }

        IEnumerable<IMapLayer> Layers { get; }

        IMapLayer GenerateNewLayer();

        void Process();

        float GetHeight(float positionX, float positionZ);
    }
}
