using Rhisis.Game.Abstractions;
using System;
using System.Collections.Generic;

namespace Rhisis.Game
{
    public class Map : IMap
    {
        private readonly List<IMapLayer> _layers;
        private readonly float[] _heights;

        public int Id { get; }

        public int Width { get; }

        public int Length { get; }

        public IEnumerable<IMapLayer> Layers => _layers;

        public Map(int id)
        {
            Id = id;
            _layers = new List<IMapLayer>();
        }

        public IMapLayer GenerateNewLayer()
        {
            throw new NotImplementedException();
        }

        public float GetHeight(float positionX, float positionZ)
        {
            if (positionX < 0 || positionZ < 0 || positionX > Width || positionZ > Length)
            {
                return 0;
            }

            int offset = (int)(positionX * Width + positionZ);

            return _heights[offset];
        }

        public void Process()
        {
            lock (_layers)
            {
                foreach (IMapLayer layer in _layers)
                {
                    layer.Process();
                }
            }
        }
    }
}
