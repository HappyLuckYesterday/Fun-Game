using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Common;
using Rhisis.World.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Initializers
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class PlayerInventoryInitializer : IGameSystemLifeCycle
    {
        private readonly IRhisisDatabase _database;
        private readonly IItemFactory _itemFactory;

        /// <summary>
        /// Gets the initialization order of the inventory system when creating a new player.
        /// </summary>
        public int Order => 0;

        public PlayerInventoryInitializer(IRhisisDatabase database, IItemFactory itemFactory)
        {
            _database = database;
            _itemFactory = itemFactory;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<InventoryItem> items = _database.ItemStorage
                .Include(x => x.Item)
                .Where(x => x.CharacterId == player.PlayerData.Id && x.StorageTypeId == (int)ItemStorageType.Inventory && !x.IsDeleted)
                .OrderBy(x => x.Slot)
                .Select(x => _itemFactory.CreateInventoryItem(x));

            player.Inventory.Initialize(items);
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            IEnumerable<DbItemStorage> dbInventoryItems = _database.ItemStorage
                .Include(x => x.Item)
                    .ThenInclude(x => x.ItemAttributes)
                .Where(x => x.CharacterId == player.PlayerData.Id && x.StorageTypeId == (int)ItemStorageType.Inventory);

            IEnumerable<DbItemStorage> itemsToDelete = (from dbItem in dbInventoryItems
                                                        let inventoryItem = player.Inventory.GetItem(x => x.DatabaseStorageItemId == dbItem.Id)
                                                        where !dbItem.IsDeleted && inventoryItem == null
                                                        select dbItem).ToList();

            foreach (DbItemStorage storedItem in itemsToDelete)
            {
                storedItem.IsDeleted = true;
                storedItem.Item.IsDeleted = true;

                _database.ItemStorage.Update(storedItem);
            }

            // Add or update items
            foreach (InventoryItem item in player.Inventory)
            {
                if (item == null || item.Id == Item.Empty)
                {
                    continue;
                }

                DbItemStorage dbItem = dbInventoryItems.FirstOrDefault(x => x.Id == item.DatabaseStorageItemId && !x.IsDeleted);

                if (dbItem != null && dbItem.Id != 0)
                {
                    dbItem.CharacterId = player.PlayerData.Id;
                    dbItem.Quantity = item.Quantity;
                    dbItem.Slot = item.Slot;
                    dbItem.Item.Refine = item.Refine;
                    dbItem.Item.Element = (byte)item.Element;
                    dbItem.Item.ElementRefine = item.ElementRefine;
                    dbItem.Updated = DateTime.UtcNow;

                    // TODO: update item attributes

                    _database.ItemStorage.Update(dbItem);
                }
                else
                {
                    dbItem = new DbItemStorage
                    {
                        CharacterId = player.PlayerData.Id,
                        StorageTypeId = (int)ItemStorageType.Inventory,
                        Quantity = item.Quantity,
                        Slot = item.Slot,
                        Updated = DateTime.UtcNow,
                        Item = new DbItem
                        {
                            CreatorId = item.CreatorId,
                            GameItemId = item.Id,
                            Refine = item.Refine,
                            Element = (byte)item.Element,
                            ElementRefine = item.ElementRefine
                            // TODO: Add item attributes
                        }
                    };

                    _database.ItemStorage.Add(dbItem);
                }
            }

            _database.SaveChanges();
        }
    }
}
