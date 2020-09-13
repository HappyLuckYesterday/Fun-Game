using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Entities.Internal;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Structures;
using System;
using System.Linq;

namespace Rhisis.World.Game.Factories.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class ItemFactory : IItemFactory
    {
        private readonly ILogger<ItemFactory> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGameResources _gameResources;
        private readonly ObjectFactory _itemEntityFactory;

        /// <summary>
        /// Creates a new <see cref="ItemFactory"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="gameResources">Game resources.</param>
        public ItemFactory(ILogger<ItemFactory> logger, IServiceProvider serviceProvider, IGameResources gameResources)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _gameResources = gameResources;
            _itemEntityFactory = ActivatorUtilities.CreateFactory(typeof(ItemEntity), Type.EmptyTypes);
        }

        /// <inheritdoc />
        public Item CreateItem(int id, int creatorId = -1) => CreateItem(id, 0, ElementType.None, 0, creatorId);

        /// <inheritdoc />
        public Item CreateItem(int id, byte refine, ElementType element, byte elementRefine, int creatorId = -1)
        {            
            if (!_gameResources.Items.TryGetValue(id, out ItemData itemData))
            {
                _logger.LogWarning($"Cannot find item data for item id: '{id}'.");
                return null;
            }

            return new Item(itemData, 1, null)
            {
                Refine = refine,
                Element = element,
                ElementRefine = elementRefine,
                CreatorId = creatorId
            };
        }

        /// <inheritdoc />
        public Item CreateItem(string name, byte refine, ElementType element, byte elementRefine, int creatorId = -1)
        {
            var itemData = _gameResources.Items.FirstOrDefault(x => x.Value.Name == name);

            if (itemData.Value is null)
            {
                _logger.LogWarning($"Cannot find item data for item name: '{name}'.");
                return null;
            }

            return new Item(itemData.Value, 1, null)
            {
                Refine = refine,
                Element = element,
                ElementRefine = elementRefine,
                CreatorId = creatorId
            };
        }

        /// <inheritdoc />
        public Item CreateItem(DbItemStorage databaseItem)
        {
            if (!_gameResources.Items.TryGetValue(databaseItem.Item.GameItemId, out ItemData itemData))
            {
                _logger.LogWarning($"Cannot find item data for item id: '{databaseItem.Item.GameItemId}'.");
                return null;
            }

            return new Item(itemData, databaseItem.Quantity, databaseItem.Id);
        }

        /// <inheritdoc />
        public InventoryItem CreateInventoryItem(DbItemStorage databaseStorageItem)
        {
            ItemData itemData = GetItemData(databaseStorageItem.Item.GameItemId);

            return itemData != null ? new InventoryItem(itemData, databaseStorageItem.Quantity, -1, databaseStorageItem.Slot, databaseStorageItem.ItemId, databaseStorageItem.Id) : null;
        }

        /// <inheritdoc />
        public IItemEntity CreateItemEntity(IMapInstance currentMapContext, IMapLayer currentMapLayerContext, Item item, IWorldEntity owner = null)
        {
            var itemEntity = _itemEntityFactory(_serviceProvider, null) as IItemEntity;

            itemEntity.Object = new ObjectComponent
            {
                CurrentMap = currentMapContext,
                MapId = currentMapContext.Id,
                LayerId = currentMapLayerContext.Id,
                ModelId = item.Id,
                Name = item.Data.Name,
                Spawned = true,
                Type = WorldObjectType.Item
            };

            itemEntity.Drop = new DropComponent
            {
                Owner = owner,
                Item = item
            };

            return itemEntity;
        }

        private ItemData GetItemData(int itemId)
        {
            if (!_gameResources.Items.TryGetValue(itemId, out ItemData itemData))
            {
                _logger.LogWarning($"Cannot find item data for item id: '{itemId}'.");
                return null;
            }

            return itemData;
        }
    }
}
