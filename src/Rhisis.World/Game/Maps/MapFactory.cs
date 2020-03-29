﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Dyo;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Maps.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.World.Game.Maps
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class MapFactory : IMapFactory
    {
        private readonly ILogger<MapFactory> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly INpcFactory _npcFactory;
        private readonly IItemFactory _itemFactory;
        private readonly IMonsterFactory _monsterFactory;

        /// <summary>
        /// Creates a new <see cref="MapFactory"/> instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="npcFactory"></param>
        public MapFactory(ILogger<MapFactory> logger, IServiceProvider serviceProvider, INpcFactory npcFactory, IItemFactory itemFactory, IMonsterFactory monsterFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _npcFactory = npcFactory;
            _itemFactory = itemFactory;
            _monsterFactory = monsterFactory;
        }

        /// <inheritdoc />
        public IMapInstance Create(string mapPath, string mapName, int mapId)
        {
            WldFileInformations worldInformations = LoadWld(mapPath, mapName);

            var mapInstance = ActivatorUtilities.CreateInstance<MapInstance>(_serviceProvider, mapId, mapName, worldInformations);

            LoadDyo(mapPath, mapInstance);
            LoadRgn(mapPath, mapInstance);

            mapInstance.CreateMapLayer();

            return mapInstance;
        }

        /// <inheritdoc />
        public IMapLayer CreateLayer(IMapInstance parentMapInstance, int layerId)
        {
            var mapLayer = ActivatorUtilities.CreateInstance<MapLayer>(_serviceProvider, parentMapInstance, layerId);

            foreach (IMapRegion region in parentMapInstance.Regions)
            {
                if (region is IMapRespawnRegion respawnRegion)
                {
                    if (respawnRegion.ObjectType == WorldObjectType.Mover)
                    {
                        for (int i = 0; i < respawnRegion.Count; i++)
                        {
                            IMonsterEntity monster = _monsterFactory.CreateMonster(parentMapInstance, mapLayer, respawnRegion.ModelId, respawnRegion);
                                
                            mapLayer.AddEntity(monster);
                        }
                    }
                    else if (respawnRegion.ObjectType == WorldObjectType.Item)
                    {
                        var item = _itemFactory.CreateItem(respawnRegion.ModelId, 0, 0, 0);

                        for (int i = 0; i < respawnRegion.Count; ++i)
                        {
                            IItemEntity itemEntity = _itemFactory.CreateItemEntity(parentMapInstance, mapLayer, item);
                            itemEntity.Object.Position = respawnRegion.GetRandomPosition();
                            itemEntity.Region = respawnRegion;

                            mapLayer.AddEntity(itemEntity);
                        }
                    }
                }
            }

            return mapLayer;
        }

        /// <summary>
        /// Loads the world map informations from the WLD file.
        /// </summary>
        private WldFileInformations LoadWld(string mapPath, string mapName)
        {
            string wldFilePath = Path.Combine(mapPath, $"{mapName}.wld");
            using var wldFile = new WldFile(wldFilePath);
            
            return wldFile.WorldInformations;
        }

        /// <summary>
        /// Load NPC from the DYO file.
        /// </summary>
        private void LoadDyo(string mapPath, IMapInstance map)
        {
            string dyo = Path.Combine(mapPath, $"{map.Name}.dyo");
            using var dyoFile = new DyoFile(dyo);
            IEnumerable<NpcDyoElement> npcElements = dyoFile.GetElements<NpcDyoElement>();

            foreach (NpcDyoElement element in npcElements)
            {
                map.AddEntity(_npcFactory.CreateNpc(map, element));
            }
        }

        /// <summary>
        /// Load regions from the RGN file.
        /// </summary>
        private void LoadRgn(string mapPath, MapInstance map)
        {
            var regions = new List<IMapRegion>();
            string rgn = Path.Combine(mapPath, $"{map.Name}.rgn");

            using (var rgnFile = new RgnFile(rgn))
            {
                IEnumerable<IMapRespawnRegion> respawnersRgn = rgnFile.GetElements<RgnRespawn7>()
                    .Select(x => MapRespawnRegion.FromRgnElement(x));

                regions.AddRange(respawnersRgn);

                foreach (RgnRegion3 region in rgnFile.GetElements<RgnRegion3>())
                {
                    switch (region.Index)
                    {
                        case RegionInfo.RI_REVIVAL:
                            int revivalMapId = map.MapInformation.RevivalMapId == 0 ? map.Id : map.MapInformation.RevivalMapId;
                            var newRevivalRegion = MapRevivalRegion.FromRgnElement(region, revivalMapId);
                            regions.Add(newRevivalRegion);
                            break;
                        case RegionInfo.RI_TRIGGER:
                            regions.Add(MapTriggerRegion.FromRgnElement(region));
                            break;

                            // TODO: load collector regions
                    }
                }
            }

            map.SetRegions(regions);
        }
    }
}
