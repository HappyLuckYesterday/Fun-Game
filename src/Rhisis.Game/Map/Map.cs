using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Factories;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.IO.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Game.Map
{
    [DebuggerDisplay("{Name} ({Id} | {Width}x{Length})")]
    public class Map : IMap
    {
        private const int DefaultMapLayerId = 1;
        private const int FrameRate = 67;
        private const float UpdateRate = 1000f / FrameRate;

        private readonly WldFileInformations _worldInformations;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEntityFactory _entityFactory;
        private readonly ILogger<Map> _logger;
        private readonly List<IMapLayer> _layers;
        private readonly List<IMapRegion> _regions;
        private readonly List<IMapObject> _mapObjects;
        private readonly float[] _heights;

        private readonly CancellationToken _mainProcessTaskCancelToken;
        private readonly CancellationTokenSource _mainProcessTaskCancelTokenSource;

        private int _mapLayerIdGenerator;
        private IMapLayer _defaultMapLayer;

        public int Id { get; }

        public string Name { get; }

        public int Width => _worldInformations.Width;

        public int Length => _worldInformations.Length;

        public int RevivalMapId => _worldInformations.RevivalMapId == 0 ? Id : _worldInformations.RevivalMapId;

        public IEnumerable<IMapLayer> Layers => _layers;

        public IEnumerable<IMapRegion> Regions => _regions;

        public IEnumerable<IMapObject> Objects => _mapObjects;

        public Map(int id, string name, WldFileInformations worldInformation, IServiceProvider serviceProvider, IEntityFactory entityFactory, ILogger<Map> logger)
        {
            Id = id;
            Name = name;
            _worldInformations = worldInformation;
            _serviceProvider = serviceProvider;
            _entityFactory = entityFactory;
            _logger = logger;
            _layers = new List<IMapLayer>();
            _regions = new List<IMapRegion>();
            _mapObjects = new List<IMapObject>();
            _heights = new float[Width * Length + 1];
            _mapLayerIdGenerator = DefaultMapLayerId;

            _mainProcessTaskCancelTokenSource = new CancellationTokenSource();
            _mainProcessTaskCancelToken = _mainProcessTaskCancelTokenSource.Token;
        }

        public IMapLayer GetMapLayer(int layerId) => _layers.FirstOrDefault(x => x.Id == layerId) ?? _defaultMapLayer;

        public IMapLayer GenerateNewLayer()
        {
            lock (_layers)
            {
                IMapLayer newMapLayer;

                if (_defaultMapLayer == null)
                {
                    _defaultMapLayer = new MapLayer(this, DefaultMapLayerId, _serviceProvider);
                    newMapLayer = _defaultMapLayer;
                }
                else
                {
                    newMapLayer = new MapLayer(this, ++_mapLayerIdGenerator, _serviceProvider);
                }

                foreach (IMapRegion region in Regions)
                {
                    if (region is IMapRespawnRegion respawnRegion)
                    {
                        if (respawnRegion.ObjectType == WorldObjectType.Mover)
                        {
                            for (int i = 0; i < respawnRegion.Count; i++)
                            {
                                IMonster monster = _entityFactory.CreateMonster(respawnRegion.ModelId, Id, newMapLayer.Id, respawnRegion.GetRandomPosition(), respawnRegion);
                                newMapLayer.AddMonster(monster);
                            }
                        }
                        else if (respawnRegion.ObjectType == WorldObjectType.Item)
                        {
                            for (int i = 0; i < respawnRegion.Count; ++i)
                            {
                                IMapItem mapItem = _entityFactory.CreateMapItem();
                                newMapLayer.AddItem(mapItem);
                            }
                        }
                    }
                }

                if (_mapObjects.Any())
                {
                    foreach (IMapObject mapObject in _mapObjects)
                    {
                        switch (mapObject)
                        {
                            case IMapNpcObject npcObject:
                                newMapLayer.AddNpc(_entityFactory.CreateNpc(npcObject, Id, newMapLayer.Id));
                                break;
                        }
                    }
                }

                _layers.Add(newMapLayer);

                return newMapLayer;
            }
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

        public void SetObjects(IEnumerable<IMapObject> mapObjects)
        {
            _mapObjects.AddRange(mapObjects);
        }

        public void StartUpdate()
        {
            Task.Factory.StartNew(Process, _mainProcessTaskCancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void StopUpdate()
        {
            _mainProcessTaskCancelTokenSource.Cancel();
        }

        public void Dispose()
        {
            StopUpdate();
            // TODO: dispose all layers
        }

        private void Process()
        {
            var currentTime = DateTime.UtcNow;

            while (!_mainProcessTaskCancelToken.IsCancellationRequested)
            {
                try
                {
                    var nextUpdate = DateTime.UtcNow.AddMilliseconds(UpdateRate);
                    Update();
                    currentTime = DateTime.UtcNow;

                    if (nextUpdate > currentTime)
                    {
                        Thread.Sleep((nextUpdate - currentTime).Milliseconds);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"An error occured on map '{Name}'.");
                }
            }
        }

        private void Update()
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
