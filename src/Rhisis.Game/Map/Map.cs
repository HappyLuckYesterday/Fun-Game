using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.IO.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Rhisis.Game.Map
{
    [DebuggerDisplay("{Name} ({Id} | {Width}x{Lenght})")]
    public class Map : IMap
    {
        private readonly WldFileInformations _worldInformations;
        private readonly List<IMapLayer> _layers;
        private readonly List<IMapRegion> _regions;
        private readonly float[] _heights;

        public int Id { get; }

        public string Name { get; }

        public int Width => _worldInformations.Width;

        public int Length => _worldInformations.Length;

        public int RevivalMapId => _worldInformations.RevivalMapId == 0 ? Id : _worldInformations.RevivalMapId;

        public IEnumerable<IMapLayer> Layers => _layers;

        public IEnumerable<IMapRegion> Regions => _regions;

        public Map(int id, string name, WldFileInformations worldInformation)
        {
            Id = id;
            Name = name;
            _worldInformations = worldInformation;
            _layers = new List<IMapLayer>();
            _regions = new List<IMapRegion>();
            _heights = new float[Width * Length + 1];
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

            var offset = (int)(positionX * Width + positionZ);

            return _heights[offset];
        }

        public void SetHeight(float positionX, float positionZ, float value)
        {
            if (positionX < 0 || positionZ < 0 || positionX > Width || positionZ > Length)
            {
                return;
            }

            var offset = (int)(positionX * Width + positionZ);

            _heights[offset] = value;
        }

        public void SetRegions(IEnumerable<IMapRegion> regions)
        {
            _regions.AddRange(regions);
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
