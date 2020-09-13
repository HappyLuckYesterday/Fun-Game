using Microsoft.EntityFrameworkCore;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems.Initializers
{
    [Injectable]
    public class PlayerInventoryInitializer : IPlayerInitializer
    {
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;

        public PlayerInventoryInitializer(IRhisisDatabase database, IGameResources gameResources)
        {
            _database = database;
            _gameResources = gameResources;
        }

        public void Load(IPlayer player)
        {
            IEnumerable<IItem> items = _database.ItemStorage
                .Include(x => x.Item)
                .Where(x => x.CharacterId == player.CharacterId && x.StorageTypeId == (int)ItemStorageType.Inventory && !x.IsDeleted)
                .OrderBy(x => x.Slot)
                .Select(x => CreateInventoryItem(x, _gameResources));

            player.Inventory.Initialize(items);
        }

        public void Save(IPlayer player)
        {
            IEnumerable<DbItemStorage> dbInventoryItems = _database.ItemStorage
                   .Include(x => x.Item)
                       .ThenInclude(x => x.ItemAttributes)
                   .Where(x => x.CharacterId == player.CharacterId && x.StorageTypeId == (int)ItemStorageType.Inventory);

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
            foreach (IItem item in player.Inventory)
            {
                if (item == null || item.Id == -1)
                {
                    continue;
                }

                DbItemStorage dbItem = dbInventoryItems.FirstOrDefault(x => x.Id == item.DatabaseStorageItemId && !x.IsDeleted);

                if (dbItem != null && dbItem.Id != 0)
                {
                    dbItem.CharacterId = player.CharacterId;
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
                        CharacterId = player.CharacterId,
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

        private static IItem CreateInventoryItem(DbItemStorage dbItemStorage, IGameResources gameResources)
        {
            int itemId = dbItemStorage.Item.GameItemId;

            if (!gameResources.Items.TryGetValue(itemId, out ItemData itemData))
            {
                throw new KeyNotFoundException($"Cannot find item data with id: '{itemId}'.");
            }

            return new Item(itemData, dbItemStorage.Id)
            {
                CreatorId = dbItemStorage.Item.CreatorId,
                Refine = dbItemStorage.Item.Refine.GetValueOrDefault(),
                Element = (ElementType)dbItemStorage.Item.Element.GetValueOrDefault(),
                ElementRefine = dbItemStorage.Item.ElementRefine.GetValueOrDefault(),
                Slot = dbItemStorage.Slot,
                Quantity = dbItemStorage.Quantity
            };
        }
    }
}
