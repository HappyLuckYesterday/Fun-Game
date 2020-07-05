﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures.Game;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Drop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Inventory
{
    /// <summary>
    /// Implements the flyff inventory system management.
    /// </summary>
    [Injectable(ServiceLifetime.Transient)]
    public sealed class InventorySystem : IInventorySystem
    {
        public const int LeftWeaponSlot = EquipOffset + (int)ItemPartType.LeftWeapon;
        public const int RightWeaponSlot = EquipOffset + (int)ItemPartType.RightWeapon;
        public const int BulletSlot = EquipOffset + (int)ItemPartType.Bullet;
        public const int EquipOffset = 42;
        public const int MaxItems = 73;
        public const int InventorySize = EquipOffset;
        public const int MaxHumanParts = MaxItems - EquipOffset;

        private readonly ILogger<InventorySystem> _logger;
        private readonly IRhisisDatabase _database;
        private readonly IItemFactory _itemFactory;
        private readonly IInventoryPacketFactory _inventoryPacketFactory;
        private readonly IInventoryItemUsage _inventoryItemUsage;
        private readonly IDropSystem _dropSystem;
        private readonly ITextPacketFactory _textPacketFactory;

        /// <summary>
        /// Gets the initialization order of the inventory system when creating a new player.
        /// </summary>
        public int Order => 0;

        /// <summary>
        /// Creates a new <see cref="InventorySystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="database">Rhisis database.</param>
        /// <param name="itemFactory">Item factory.</param>
        /// <param name="inventoryPacketFactory">Inventory packet factory.</param>
        /// <param name="inventoryItemUsage">Inventory item usage system.</param>
        /// <param name="dropSystem">Drop system.</param>
        /// <param name="textPacketFactory">Text packet factory.</param>
        public InventorySystem(ILogger<InventorySystem> logger, IRhisisDatabase database, IItemFactory itemFactory, IInventoryPacketFactory inventoryPacketFactory, IInventoryItemUsage inventoryItemUsage, IDropSystem dropSystem, ITextPacketFactory textPacketFactory)
        {
            _logger = logger;
            _database = database;
            _itemFactory = itemFactory;
            _inventoryPacketFactory = inventoryPacketFactory;
            _inventoryItemUsage = inventoryItemUsage;
            _dropSystem = dropSystem;
            _textPacketFactory = textPacketFactory;
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
            //DbCharacter character = _database.Characters.Include(x => x.Items).FirstOrDefault(x => x.Id == player.PlayerData.Id);
            //IEnumerable<DbItem> itemsToDelete = (from dbItem in character.Items
            //                                     let inventoryItem = player.Inventory.GetItem(x => x.DatabaseItemId == dbItem.Id)
            //                                     where !dbItem.IsDeleted && inventoryItem == null
            //                                     select dbItem).ToList();

            //foreach (DbItem dbItem in itemsToDelete)
            //{
            //    dbItem.IsDeleted = true;

            //    _database.Items.Update(dbItem);
            //}

            // Add or update items
            foreach (Item item in player.Inventory)
            {
                if (item == null || item.Id == Item.Empty)
                {
                    continue;
                }

                //DbItem dbItem = character.Items.FirstOrDefault(x => x.Id == item.DatabaseItemId && !x.IsDeleted);

                //if (dbItem != null && dbItem.Id != 0)
                //{
                //    dbItem.CharacterId = player.PlayerData.Id;
                //    dbItem.GameItemId = item.Id;
                //    dbItem.ItemCount = item.Quantity;
                //    dbItem.ItemSlot = item.Slot;
                //    dbItem.Refine = item.Refine;
                //    dbItem.Element = (byte)item.Element;
                //    dbItem.ElementRefine = item.ElementRefine;

                //    _database.Items.Update(dbItem);
                //}
                //else
                //{
                //    dbItem = new DbItem
                //    {
                //        CharacterId = player.PlayerData.Id,
                //        CreatorId = item.CreatorId,
                //        GameItemId = item.Id,
                //        ItemCount = item.Quantity,
                //        ItemSlot = item.Slot,
                //        Refine = item.Refine,
                //        Element = (byte)item.Element,
                //        ElementRefine = item.ElementRefine
                //    };

                //    _database.Items.Add(dbItem);
                //}
            }

            _database.SaveChanges();
        }

        /// <inheritdoc />
        public int CreateItem(IPlayerEntity player, int itemId, int quantity, int creatorId = -1, bool sendToPlayer = true)
        {
            Item itemToCreate = _itemFactory.CreateItem(itemId);

            return CreateItem(player, itemToCreate, quantity, creatorId, sendToPlayer);
        }

        /// <inheritdoc />
        public int CreateItem(IPlayerEntity player, Item item, int quantity, int creatorId = -1, bool sendToPlayer = true)
        {
            item.Quantity = quantity;

            IEnumerable<ItemCreationResult> creationResult = player.Inventory.AddItem(item);

            if (creationResult.Any())
            {
                if (sendToPlayer)
                {
                    foreach (ItemCreationResult itemResult in creationResult)
                    {
                        if (itemResult.ActionType == ItemCreationActionType.Add)
                        {
                            _inventoryPacketFactory.SendItemCreation(player, itemResult.Item);
                        }
                        else if (itemResult.ActionType == ItemCreationActionType.Update)
                        {
                            _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, itemResult.Item.Index, itemResult.Item.Quantity);
                        }
                    }
                }
            }
            else
            {
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
            }

            return creationResult.Sum(x => x.Item.Quantity);
        }

        /// <inheritdoc />
        public int DeleteItem(IPlayerEntity player, int itemUniqueId, int quantity, bool sendToPlayer = true)
        {
            if (quantity <= 0)
            {
                return 0;
            }

            InventoryItem itemToDelete = player.Inventory.GetItemAtIndex(itemUniqueId);

            if (itemToDelete == null)
            {
                throw new ArgumentNullException(nameof(itemToDelete), $"Cannot find item with unique id: '{itemUniqueId}' in '{player.Object.Name}''s inventory.");
            }

            return DeleteItem(player, itemToDelete, quantity, sendToPlayer);
        }

        /// <inheritdoc />
        public int DeleteItem(IPlayerEntity player, InventoryItem itemToDelete, int quantity, bool sendToPlayer = true)
        {
            int quantityToDelete = Math.Min(itemToDelete.Quantity, quantity);

            itemToDelete.Quantity -= quantityToDelete;

            if (sendToPlayer)
            {
                _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, itemToDelete.Index, itemToDelete.Quantity);
            }

            if (itemToDelete.Quantity <= 0)
            {
                if (player.Inventory.IsItemEquiped(itemToDelete))
                {
                    player.Inventory.Unequip(itemToDelete);
                }

                player.Inventory.RemoveItem(itemToDelete);
            }

            return quantityToDelete;
        }

        /// <inheritdoc />
        public void MoveItem(IPlayerEntity player, byte sourceSlot, byte destinationSlot, bool sendToPlayer = true)
        {
            if (sourceSlot < 0 || sourceSlot >= MaxItems)
            {
                throw new InvalidOperationException("Source slot is out of inventory range.");
            }

            if (destinationSlot < 0 || destinationSlot >= MaxItems)
            {
                throw new InvalidOperationException("Destination slot is out of inventory range.");
            }

            if (sourceSlot == destinationSlot)
            {
                throw new InvalidOperationException("Cannot move an item to the same slot.");
            }

            InventoryItem sourceItem = player.Inventory.GetItemAtSlot(sourceSlot);

            if (sourceItem == null)
            {
                throw new InvalidOperationException("Source item not found");
            }

            InventoryItem destinationItem = player.Inventory.GetItemAtSlot(destinationSlot);

            if (destinationItem != null && sourceItem.Id == destinationItem.Id && sourceItem.Data.IsStackable)
            {
                int newQuantity = sourceItem.Quantity + destinationItem.Quantity;

                if (newQuantity > destinationItem.Data.PackMax)
                {
                    destinationItem.Quantity = destinationItem.Data.PackMax;
                    sourceItem.Quantity = newQuantity - sourceItem.Data.PackMax;

                    _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, sourceItem.Index, sourceItem.Quantity);
                    _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, destinationItem.Index, destinationItem.Quantity);
                }
                else
                {
                    destinationItem.Quantity = newQuantity;
                    DeleteItem(player, sourceItem.Index, sourceItem.Quantity);
                    _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, destinationItem.Index, destinationItem.Quantity);
                }
            }
            else
            {
                player.Inventory.Swap(sourceSlot, destinationSlot);

                if (sendToPlayer)
                {
                    _inventoryPacketFactory.SendItemMove(player, sourceSlot, destinationSlot);
                }
            }
        }

        /// <inheritdoc />
        public bool EquipItem(IPlayerEntity player, int itemUniqueId, int equipPart)
        {
            InventoryItem itemToEquip = player.Inventory.GetItemAtIndex(itemUniqueId);

            if (itemToEquip == null)
            {
                throw new ArgumentNullException(nameof(itemToEquip), $"Cannot find item with unique id: '{itemUniqueId}' in {player.Object.Name} inventory.");
            }

            bool isItemEquiped = player.Inventory.IsItemEquiped(itemToEquip);

            if (isItemEquiped)
            {
                if (player.Inventory.Unequip(itemToEquip))
                {
                    _inventoryPacketFactory.SendItemEquip(player, itemToEquip, (int)itemToEquip.Data.Parts, false);

                    _logger.LogDebug($"Unequip {itemToEquip} to slot {itemToEquip.Slot}");
                }
            }
            else
            {
                if (!IsItemEquipable(player, itemToEquip))
                {
                    return false;
                }

                InventoryItem equipedItem = player.Inventory.GetEquipedItem(itemToEquip.Data.Parts);

                if (equipedItem != null)
                {
                    _logger.LogDebug($"Unequip {equipedItem} and equip {itemToEquip}");

                    if (player.Inventory.Unequip(equipedItem))
                    {
                        _logger.LogDebug($"Unequip {equipedItem} to slot {equipedItem.Slot}");
                    }
                }

                ItemPartType destinationPart = itemToEquip.Data.Parts;
                // TODO: check dual weapon

                if (player.Inventory.Equip(itemToEquip, destinationPart))
                {
                    _inventoryPacketFactory.SendItemEquip(player, itemToEquip, (int)itemToEquip.Data.Parts, true);

                    _logger.LogDebug($"Equip {itemToEquip}");
                }
            }

            return true;
        }

        /// <inheritdoc />
        public void UseItem(IPlayerEntity player, int itemUniqueId, int part)
        {
            InventoryItem itemToUse = player.Inventory.GetItemAtIndex(itemUniqueId);

            if (itemToUse == null)
            {
                throw new ArgumentNullException(nameof(itemToUse), $"Cannot find item with unique id: '{itemUniqueId}' in {player.Object.Name} inventory.");
            }

            if (part >= 0)
            {
                if (part >= MaxHumanParts)
                {
                    throw new InvalidOperationException($"Invalid equipement part.");
                }

                if (!player.Battle.IsFighting)
                {
                    EquipItem(player, itemUniqueId, part);
                }
            }
            else
            {
                if (itemToUse.Data.IsUseable && itemToUse.Quantity > 0)
                {
                    _logger.LogTrace($"{player.Object.Name} want to use {itemToUse.Data.Name}.");

                    if (player.Inventory.ItemHasCoolTime(itemToUse) && !player.Inventory.CanUseItemWithCoolTime(itemToUse))
                    {
                        _logger.LogDebug($"Player '{player.Object.Name}' cannot use item {itemToUse.Data.Name}: CoolTime.");
                        return;
                    }

                    switch (itemToUse.Data.ItemKind2)
                    {
                        case ItemKind2.REFRESHER:
                        case ItemKind2.POTION:
                        case ItemKind2.FOOD:
                            _inventoryItemUsage.UseFoodItem(player, itemToUse);
                            break;
                        case ItemKind2.MAGIC:
                            _inventoryItemUsage.UseMagicItem(player, itemToUse);
                            break;
                        case ItemKind2.SYSTEM:
                            UseSystemItem(player, itemToUse);
                            break;
                        case ItemKind2.GMTEXT:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.GENERAL:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.BUFF:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.BUFF2:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.AIRFUEL:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.FURNITURE:
                        case ItemKind2.PAPERING:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.GUILDHOUSE_FURNITURE:
                        case ItemKind2.GUILDHOUSE_NPC:
                        case ItemKind2.GUILDHOUSE_PAPERING:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.GUILDHOUES_COMEBACK:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.BLINKWING:
                            _inventoryItemUsage.UseBlinkwingItem(player, itemToUse);
                            break;
                        default:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                    }
                }
            }
        }

        /// <inheritdoc />
        public void UseSystemItem(IPlayerEntity player, InventoryItem systemItem)
        {
            switch (systemItem.Data.ItemKind3)
            {
                case ItemKind3.SCROLL:
                    UseScrollItem(player, systemItem);
                    break;
                default:
                    _logger.LogDebug($"Item usage for {systemItem.Data.ItemKind3} is not implemented.");
                    _textPacketFactory.SendSnoop(player, $"Item usage for {systemItem.Data.ItemKind3} is not implemented.");
                    break;
            }
        }

        /// <inheritdoc />
        public void UseScrollItem(IPlayerEntity player, InventoryItem scrollItem)
        {
            switch (scrollItem.Data.Id)
            {
                case DefineItem.II_SYS_SYS_SCR_PERIN:
                    _inventoryItemUsage.UsePerin(player, scrollItem);
                    break;
                default:
                    _logger.LogDebug($"Item usage for {scrollItem.Data.Id} is not implemented.");
                    _textPacketFactory.SendSnoop(player, $"Item usage for {scrollItem.Data.Id} is not implemented.");
                    break;
            }
        }

        /// <inhertidoc />
        public void DropItem(IPlayerEntity player, int itemUniqueId, int quantity)
        {
            InventoryItem itemToDrop = player.Inventory.GetItemAtIndex(itemUniqueId);

            if (itemToDrop == null)
            {
                throw new ArgumentNullException(nameof(itemToDrop), $"Cannot find item with unique id: '{itemUniqueId}' in {player.Object.Name} inventory.");
            }

            if (itemToDrop.Slot >= EquipOffset)
            {
                throw new InvalidOperationException($"Cannot drop an equiped item.");
            }

            int quantityToDrop = Math.Min(quantity, itemToDrop.Quantity);

            if (quantityToDrop <= 0)
            {
                throw new InvalidOperationException("Cannot drop a zero or negative quantit.");
            }

            _dropSystem.DropItem(player, itemToDrop, owner: null, quantity: quantityToDrop);
            DeleteItem(player, itemUniqueId, quantityToDrop);
        }

        /// <summary>
        /// Check if the given item is equipable by a player.
        /// </summary>
        /// <param name="player">Player trying to equip an item.</param>
        /// <param name="item">Item to equip.</param>
        /// <returns>True if the player can equip the item; false otherwise.</returns>
        private bool IsItemEquipable(IPlayerEntity player, InventoryItem item)
        {
            if (item.Data.ItemSex != int.MaxValue && item.Data.ItemSex != player.VisualAppearance.Gender)
            {
                _logger.LogTrace("Wrong sex for armor");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_WRONGSEX, item.Data.Name);
                return false;
            }

            if (player.Object.Level < item.Data.LimitLevel)
            {
                _logger.LogTrace("Player level to low");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REQLEVEL, item.Data.LimitLevel.ToString());
                return false;
            }

            if (!player.PlayerData.JobData.IsAnteriorJob(item.Data.ItemJob))
            {
                _logger.LogTrace($"Player {player} tried to equip '{item.Data.Name}', but doesn't have the required job: '{item.Data.ItemJob}'.");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_WRONGJOB);
                return false;
            }

            Item equipedItem = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

            if (item.Data.ItemKind3 == ItemKind3.ARROW && (equipedItem == null || equipedItem.Data.ItemKind3 != ItemKind3.BOW))
            {
                _logger.LogTrace($"Player {player} tried to equip arrows without having a bow equiped.");
                return false;
            }

            return true;
        }
    }
}
