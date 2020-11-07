using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.DependencyInjection.Extensions;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Factories;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Entities;
using Rhisis.Game.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Factories
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class EntityFactory : IEntityFactory
    {
        // TODO: move to configuration
        private const int DropGoldLimit1 = 9;
        private const int DropGoldLimit2 = 49;
        private const int DropGoldLimit3 = 99;

        private readonly ILogger<EntityFactory> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameResources _gameResources;
        private readonly IMapManager _mapManager;
        private readonly IBehaviorManager _behaviorManager;
        private readonly WorldConfiguration _worldServerConfiguration;

        public EntityFactory(ILogger<EntityFactory> logger, IServiceProvider serviceProvider, IGameResources gameResources, IMapManager mapManager, IBehaviorManager behaviorManager, IOptions<WorldConfiguration> worldServerConfiguration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _gameResources = gameResources;
            _mapManager = mapManager;
            _behaviorManager = behaviorManager;
            _worldServerConfiguration = worldServerConfiguration.Value;
        }

        public IMapItem CreateMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, Vector3 position)
        {
            return new MapItem(item, MapItemType.DropItem, mapLayer)
            {
                Owner = owner,
                Position = position.Clone(),
                Spawned = true
            };
        }

        public IMapItem CreateTemporaryMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, Vector3 position, int despawnTime)
        {
            return new MapItem(item, MapItemType.DropItem, mapLayer)
            {
                Owner = owner,
                Position = position.Clone(),
                Spawned = true,
                DespawnTime = despawnTime
            };
        }

        public IMapItem CreatePermanentMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, IMapRespawnRegion region)
        {
            return new MapItem(item, MapItemType.QuestItem, mapLayer)
            {
                Owner = owner,
                Position = region.GetRandomPosition(),
                Spawned = true,
                RespawnRegion = region
            };
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
            monster.Attributes = _serviceProvider.CreateInstance<Attributes>(monster);
            monster.Battle = _serviceProvider.CreateInstance<Battle>(monster);
            monster.Health = _serviceProvider.CreateInstance<Health>(monster);
            monster.Health.Hp = moverData.AddHp;
            monster.Health.Mp = moverData.AddMp;

            monster.Statistics = _serviceProvider.CreateInstance<Statistics>();
            monster.Statistics.Strength = moverData.Strength;
            monster.Statistics.Stamina = moverData.Stamina;
            monster.Statistics.Dexterity = moverData.Dexterity;
            monster.Statistics.Intelligence = moverData.Intelligence;

            monster.Defense = _serviceProvider.CreateInstance<Defense>(monster);
            monster.Defense.Update();

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
                npc.Shop = new ItemContainer<Item>[npcShopData.Items.Length];

                for (var i = 0; i < npcShopData.Items.Length; i++)
                {
                    npc.Shop[i] = new ItemContainer<Item>(NpcShopItemsPerTab);

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

        public IItem CreateItem(int itemId, byte refine, ElementType element, byte elementRefine, int creatorId = -1, int quantity = 1)
        {
            if (!_gameResources.Items.TryGetValue(itemId, out ItemData itemData))
            {
                _logger.LogError($"Failed to find item with id: {itemId}");
                return null;
            }

            return new Item(itemData, quantity: quantity)
            {
                Refine = refine,
                Element = element,
                ElementRefine = elementRefine,
                CreatorId = creatorId
            };
        }

        public IItem CreateGoldItem(int amount)
        {
            int goldItemId = DefineItem.II_GOLD_SEED1;
            int gold = amount * _worldServerConfiguration.Rates.Gold;

            if (gold <= 0)
            {
                throw new InvalidOperationException("Cannot create gold item with a quantity under or equal to zero.");
            }

            if (gold > (DropGoldLimit1 * _worldServerConfiguration.Rates.Gold))
            {
                goldItemId = DefineItem.II_GOLD_SEED2;
            }
            else if (gold > (DropGoldLimit2 * _worldServerConfiguration.Rates.Gold))
            {
                goldItemId = DefineItem.II_GOLD_SEED3;
            }
            else if (gold > (DropGoldLimit3 * _worldServerConfiguration.Rates.Gold))
            {
                goldItemId = DefineItem.II_GOLD_SEED4;
            }

            IItem goldItem = CreateItem(goldItemId, 0, ElementType.None, 0, 0, gold);

            if (goldItem == null)
            {
                throw new InvalidOperationException("Failed to create a gold item.");
            }

            return goldItem;
        }
    }
}
