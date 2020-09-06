using System.Collections.Generic;

namespace Rhisis.Game.Abstractions
{
    public interface IMap
    {
        IEnumerable<IMapLayer> Layers { get; }

        IMapLayer GenerateNewLayer();

        void Process();

        float GetHeight(float positionX, float positionZ);
    }
}
