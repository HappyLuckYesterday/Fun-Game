﻿using System;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;
using System.Linq;
using Rhisis.Core.Data;
using Rhisis.Core.IO;
using Sylver.Network.Data;

namespace Rhisis.World.Game.Components
{
    public class ItemContainerComponent
    {
        private const int MaxItemCoolTimes = 3;
        private readonly long[] _itemsCoolTimes = new long[MaxItemCoolTimes];

        /// <summary>
        /// Gets the <see cref="ItemContainerComponent"/> max capacity.
        /// </summary>
        public int MaxCapacity { get; }

        /// <summary>
        /// Gets the <see cref="ItemContainerComponent"/> max storage capacity.
        /// </summary>
        public int MaxStorageCapacity { get; }

        /// <summary>
        /// Gets the list of items in this <see cref="ItemContainerComponent"/>.
        /// </summary>
        public List<Item> Items { get; }

        /// <summary>
        /// Gets or sets the delayed action id of the current item behing used.
        /// </summary>
        public Guid ItemInUseActionId { get; set; }

        /// <summary>
        /// Gets the item by slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public Item this[int slot]
        {
            get => Items[slot];
            set => Items[slot] = value;
        }

        /// <summary>
        /// Creates a new <see cref="ItemContainerComponent"/> instance.
        /// </summary>
        public ItemContainerComponent(int maxCapacity) :
            this(maxCapacity, maxCapacity)
        { }

        /// <summary>
        /// Creates a new <see cref="ItemContainerComponent"/> instance.
        /// </summary>
        public ItemContainerComponent(int maxCapacity, int maxStorageCapacity)
        {
            MaxCapacity = maxCapacity;
            MaxStorageCapacity = maxStorageCapacity;
            Items = new List<Item>(new Item[MaxCapacity]);
            Reset();
        }

        /// <summary>
        /// Gets the valid items count
        /// </summary>
        /// <returns></returns>
        public int GetItemCount()
        {
            var count = 0;

            for (var i = 0; i < MaxStorageCapacity; i++)
            {
                if (Items[i] != null && Items[i].Slot != -1)
                    count++;
            }

            return count;
        }

        /// <summary>
        /// Gets the item by the unique id.
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public Item GetItem(int uniqueId) => Items.FirstOrDefault(x => x.UniqueId == uniqueId);

        /// <summary>
        /// Gets the item by the given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Item GetItem(Func<Item, bool> predicate) => Items.FirstOrDefault(predicate);

        /// <summary>
        /// Gets the items matching the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Collection of <see cref="Item"/>.</returns>
        public IEnumerable<Item> GetItems(Func<Item, bool> predicate) => Items.Where(predicate);

        /// <summary>
        /// Gets the item by item id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Item GetItemById(int itemId) => GetItem(x => x.Data != null && x.Data.Id == itemId);

        /// <summary>
        /// Gets the items matching the item id.
        /// </summary>
        /// <param name="itemId">Item id.</param>
        /// <returns>Colletion of <see cref="Item"/>.</returns>
        public IEnumerable<Item> GetItemsById(int itemId) => GetItems(x => x.Id == itemId);

        /// <summary>
        /// Gets whether the container contains at least one item of the specified item id.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public bool HasItem(int itemId)
        {
            var item = GetItemById(itemId);

            return item == null ? false : item.Quantity > 0;
        }

        /// <summary>
        /// Returs the position of an available slot.
        /// </summary>
        /// <returns></returns>
        public int GetAvailableSlot()
        {
            for (var i = 0; i < MaxStorageCapacity; i++)
            {
                if (Items[i] != null && Items[i].Slot == -1)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Check if there is any available slots.
        /// </summary>
        /// <returns></returns>
        public bool HasAvailableSlots() => GetAvailableSlot() != -1;

        /// <summary>
        /// Checks if the given slot is available.
        /// </summary>
        /// <param name="slot">Slot.</param>
        /// <returns>True if the slot is available; false otherwise.</returns>
        public bool IsSlotAvailable(int slot)
        {
            if (slot < 0 || slot >= MaxStorageCapacity)
            {
                throw new ArgumentOutOfRangeException(nameof(slot), $"The slot '{slot}' is out of range.");
            }

            return Items[slot] != null && Items[slot].UniqueId != -1;
        }

        /// <summary>
        /// Creates an item in this item container.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CreateItem(Item item)
        {
            if (item?.Data == null)
                return false;

            int availableSlot = GetAvailableSlot();

            if (availableSlot < 0)
                return false;

            item.Slot = availableSlot;
            item.UniqueId = Items[availableSlot].UniqueId;
            Items[availableSlot] = item;

            return true;
        }

        /// <summary>
        /// Serialize the ItemContainer.
        /// </summary>
        /// <param name="packet"></param>
        public void Serialize(INetPacketStream packet)
        {
            for (var i = 0; i < MaxCapacity; ++i)
                packet.Write(Items[i].UniqueId);

            packet.Write((byte)Items.Count(x => x.Id != -1));

            for (var i = 0; i < MaxCapacity; ++i)
            {
                if (Items[i].Id > 0)
                {
                    packet.Write((byte)Items[i].Slot);
                    Items[i].Serialize(packet);
                }
            }

            for (var i = 0; i < MaxCapacity; ++i)
                packet.Write(Items[i].UniqueId);
        }

        /// <summary>
        /// Gets the item cool time group.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns>Returns the item cool time group.</returns>
        public int? GetItemCoolTimeGroup(Item item)
        {
            if (item.Data.CoolTime <= 0)
                return null;

            return item.Data.ItemKind2 switch
            {
                ItemKind2.FOOD => item.Data.ItemKind3 == ItemKind3.PILL ? 1 : 0,
                ItemKind2.SKILL => 2,
                _ => null,
            };
        }

        /// <summary>
        /// Checks if the item has a cooltime.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ItemHasCoolTime(Item item) => GetItemCoolTimeGroup(item).HasValue;

        /// <summary>
        /// Check if the given item is a cooltime item and can be used.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns>Returns true if the item with cooltime can be used; false otherwise.</returns>
        public bool CanUseItemWithCoolTime(Item item)
        {
            int? group = GetItemCoolTimeGroup(item);

            return group.HasValue && _itemsCoolTimes[group.Value] < Time.GetElapsedTime();
        }

        /// <summary>
        /// Sets item cool time.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="cooltime">Cooltime.</param>
        public void SetCoolTime(Item item, int cooltime)
        {
            int? group = GetItemCoolTimeGroup(item);

            if (group.HasValue)
                _itemsCoolTimes[group.Value] = Time.GetElapsedTime() + cooltime;
        }

        /// <summary>
        /// Resets the item container.
        /// </summary>
        public void Reset()
        {
            for (var i = 0; i < MaxCapacity; i++)
            {
                Items[i] = new Item
                {
                    UniqueId = i
                };
            }
        }
    }
}
