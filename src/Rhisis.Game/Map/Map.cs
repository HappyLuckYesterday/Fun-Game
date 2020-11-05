using Microsoft.Extensions.Logging;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
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
        private const int RegionSize = 128;

        private readonly WldFileInformations _worldInformations;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEntityFactory _entityFactory;
        private readonly ILogger<Map> _logger;
        private readonly List<IMapLayer> _layers;
        private readonly List<IMapRegion> _regions;
        private readonly List<IMapObject> _mapObjects;
        private readonly float[] _heights;
        private readonly Rectangle _bounds;

        private readonly CancellationToken _mainProcessTaskCancelToken;
        private readonly CancellationTokenSource _mainProcessTaskCancelTokenSource;

        private int _mapLayerIdGenerator;
        private IMapLayer _defaultMapLayer;

        public int Id { get; }

        public string Name { get; }

        public int Width => _worldInformations.Width;

        public int Length => _worldInformations.Length;

        public int RevivalMapId => _worldInformations.RevivalMapId == 0 ? Id : _worldInformations.RevivalMapId;

        public IMapRevivalRegion DefaultRevivalRegion { get; private set; }

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
            _bounds = new Rectangle(0, 0, Width * _worldInformations.MPU * RegionSize, Length * _worldInformations.MPU * RegionSize);

            _mainProcessTaskCancelTokenSource = new CancellationTokenSource();
            _mainProcessTaskCancelToken = _mainProcessTaskCancelTokenSource.Token;
        }

        public IMapLayer GetMapLayer(int layerId) => _layers.FirstOrDefault(x => x.Id == layerId) ?? _defaultMapLayer;

        public IMapLayer GetDefaultMapLayer() => _defaultMapLayer; 

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
                            IItem item = _entityFactory.CreateItem(respawnRegion.ModelId, 0, ElementType.None, 0);

                            for (int i = 0; i < respawnRegion.Count; ++i)
                            {
                                IMapItem mapItem = _entityFactory.CreateMapItem(item, newMapLayer, null, respawnRegion.GetRandomPosition());
                                mapItem.Spawned = true;
                                // TODO: add respawn region

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
                                INpc npc = _entityFactory.CreateNpc(npcObject, Id, newMapLayer.Id);

                                if (npc != null)
                                {
                                    newMapLayer.AddNpc(npc);
                                }
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
            if (!regions.Any(x => x is IMapRevivalRegion))
            {
                // Loads the default revival region if no revival region is loaded.
                DefaultRevivalRegion = new MapRevivalRegion(0, 0, 0, 0,
                    _worldInformations.RevivalMapId, _worldInformations.RevivalKey, null, false, false);
            }

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

        public bool IsInBounds(float x, float y, float z) => _bounds.Contains(x, y, z);

        public bool IsInBounds(Vector3 position) => IsInBounds(position.X, position.Y, position.Z);

        public IMapRevivalRegion GetNearRevivalRegion(Vector3 position) => GetNearRevivalRegion(position, false);

        public IMapRevivalRegion GetNearRevivalRegion(Vector3 position, bool isChaoMode)
        {
            IEnumerable<IMapRevivalRegion> revivalRegions = Regions.Where(x => x is IMapRevivalRegion).Cast<IMapRevivalRegion>();
            var nearestRevivalRegion = revivalRegions.FirstOrDefault(x => x.MapId == Id && x.IsChaoRegion == isChaoMode && x.Contains(position) && x.TargetRevivalKey);

            if (nearestRevivalRegion != null)
                return GetRevivalRegion(nearestRevivalRegion.Key, isChaoMode);

            revivalRegions = from x in Regions
                             where x is IMapRevivalRegion y && y.IsChaoRegion == isChaoMode && !y.TargetRevivalKey
                             let region = x as IMapRevivalRegion
                             let distance = position.GetDistance3D(region.RevivalPosition)
                             orderby distance ascending
                             select region;

            return revivalRegions.FirstOrDefault() ?? DefaultRevivalRegion;
        }

        public IMapRevivalRegion GetRevivalRegion(string revivalKey) => GetRevivalRegion(revivalKey, false);

        public IMapRevivalRegion GetRevivalRegion(string revivalKey, bool isChaoMode)
        {
            IEnumerable<IMapRevivalRegion> revivalRegions = Regions.Where(x => x is IMapRevivalRegion).Cast<IMapRevivalRegion>();
            IEnumerable<IMapRevivalRegion> revivalRegion = from x in revivalRegions
                                                           where x.Key.Equals(revivalKey, StringComparison.OrdinalIgnoreCase) && x.IsChaoRegion == isChaoMode && !x.TargetRevivalKey
                                                           select x;

            return revivalRegion.FirstOrDefault() ?? DefaultRevivalRegion;
        }
    }
}
