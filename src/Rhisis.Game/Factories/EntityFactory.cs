using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Factories;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Factories
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class EntityFactory : IEntityFactory
    {
        private readonly ILogger<EntityFactory> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameResources _gameResources;
        private readonly IMapManager _mapManager;
        private readonly IBehaviorManager _behaviorManager;

        public EntityFactory(ILogger<EntityFactory> logger, IServiceProvider serviceProvider, IGameResources gameResources, IMapManager mapManager, IBehaviorManager behaviorManager)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _gameResources = gameResources;
            _mapManager = mapManager;
            _behaviorManager = behaviorManager;
        }

        public IMapItem CreateMapItem()
        {
            return null;
        }

        public IMonster CreateMonster(int moverId, int mapId, int mapLayerId, Vector3 position, IMapRespawnRegion respawnRegion)
        {
            if (!_gameResources.Movers.TryGetValue(moverId, out MoverData moverData))
            {
                _logger.LogError($"Cannot find mover data for mover: '{moverId}'.");
                return null;
            }

            IMap map = _mapManager.GetMap(mapId);

            if (map == null)
            {
                _logger.LogError($"Cannot find map with id: '{mapId}'.");
                return null;
            }

            var monster = new Monster
            {
                Data = moverData,
                Map = map,
                MapLayer = map.GetMapLayer(mapLayerId),
                Position = position.Clone(),
                RespawnRegion = respawnRegion,
                ObjectState = ObjectState.OBJSTA_STAND,
                Angle = RandomHelper.FloatRandom(0, 360f),
                Size = GameConstants.DefaultObjectSize,
                Spawned = true,
                Systems = _serviceProvider
            };
            monster.Health.Hp = moverData.AddHp;
            monster.Health.Mp = moverData.AddMp;
            monster.Statistics.Strength = moverData.Strength;
            monster.Statistics.Stamina = moverData.Stamina;
            monster.Statistics.Dexterity = moverData.Dexterity;
            monster.Statistics.Intelligence = moverData.Intelligence;
            monster.Behavior = _behaviorManager.GetDefaultBehavior(BehaviorType.Monster, monster);

            if (monster.Data.Class == MoverClassType.RANK_BOSS)
            {
                monster.Size *= 2;
            }

            return monster;
        }

        public INpc CreateNpc(IMapNpcObject npcObject, int mapId, int mapLayerId)
        {
            if (!_gameResources.Npcs.TryGetValue(npcObject.Name, out NpcData npcData))
            {
                _logger.LogError($"Cannot find mover data for NPC: '{npcObject.Name}'.");
                return null;
            }

            IMap map = _mapManager.GetMap(mapId);

            if (map == null)
            {
                _logger.LogError($"Cannot find map with id: '{mapId}'.");
                return null;
            }

            var npc = new Npc
            {
                Data = npcData,
                Map = map,
                MapLayer = map.GetMapLayer(mapLayerId),
                Position = npcObject.Position.Clone(),
                ObjectState = ObjectState.OBJSTA_STAND,
                Angle = npcObject.Angle,
                Key = npcObject.Name,
                ModelId = npcObject.ModelId,
                Size = GameConstants.DefaultObjectSize,
                Spawned = true,
                Systems = _serviceProvider
            };
            npc.Behavior = _behaviorManager.GetDefaultBehavior(BehaviorType.Npc, npc);
            npc.Quests = _gameResources.Quests.Values
                .Where(x => !string.IsNullOrEmpty(x.StartCharacter) && x.StartCharacter.Equals(npc.Key, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (npcData.HasShop)
            {
                const int NpcShopItemsPerTab = 100;
                ShopData npcShopData = npcData.Shop;
                npc.Shop = new ItemContainerComponent<Item>[npcShopData.Items.Length];

                for (var i = 0; i < npcShopData.Items.Length; i++)
                {
                    npc.Shop[i] = new ItemContainerComponent<Item>(NpcShopItemsPerTab);

                    IEnumerable<Item> shopTabItems = npcShopData.Items[i].Select((item, index) =>
                    {
                        ItemData itemData = _gameResources.Items.GetValueOrDefault(item.Id);
                        Item shopItem = new Item(itemData)
                        {
                            Refine = item.Refine,
                            Element = item.Element,
                            ElementRefine = item.ElementRefine,
                            Slot = index,
                            Quantity = itemData.PackMax
                        };

                        return shopItem;
                    });

                    npc.Shop[i].Initialize(shopTabItems.Take(NpcShopItemsPerTab));
                }
            }

            return npc;
        }
    }
}
