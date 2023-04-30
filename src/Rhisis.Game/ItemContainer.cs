using Rhisis.Core.Extensions;
using Rhisis.Game.Common;
using Rhisis.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public class ItemContainer : IEnumerable<ItemContainerSlot>
{
    protected readonly List<ItemContainerSlot> _items;

    /// <summary>
    /// Gets the number of items inside the container.
    /// </summary>
    public int Count => _items.Count(x => x.HasItem);

    /// <summary>
    /// Gets the container capacity.
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Gets the container extra capacity.
    /// </summary>
    public int ExtraCapacity { get; }

    /// <summary>
    /// Gets the container maximum capacity.
    /// </summary>
    public int MaxCapacity => Capacity + ExtraCapacity;

    public ItemContainer(int capacity, int extraCapacity = 0)
    {
        Capacity = capacity;
        ExtraCapacity = extraCapacity;
        _items = Enumerable.Repeat((ItemContainerSlot)null, MaxCapacity).ToList();

        for (int i = 0; i < MaxCapacity; i++)
        {
            _items[i] = new()
            {
                Index = i,
                Slot = i < Capacity ? i : -1,
                Item = null
            };
        }
    }

    /// <summary>
    /// Initializes the container slots.
    /// </summary>
    /// <param name="items">Dictionary of items where key is the slot and value the item.</param>
    public void Initialize(IReadOnlyDictionary<int, Item> items)
    {
        foreach (KeyValuePair<int, Item> item in items)
        {
            int itemSlot = item.Key;

            if (itemSlot < MaxCapacity)
            {
                _items[itemSlot].Item = item.Value;
                _items[itemSlot].Slot = itemSlot;
            }
        }
    }

    /// <summary>
    /// Gets an item slot matching the given item idnex.
    /// </summary>
    /// <param name="index">Item index.</param>
    /// <returns>The item slot.</returns>
    /// <exception cref="InvalidOperationException">The item index is out of range of the container's maximum capacity.</exception>
    public ItemContainerSlot GetAtIndex(int index)
    {
        if (index < 0 || index >= MaxCapacity)
        {
            throw new InvalidOperationException($"Item index is out of range: '{index}'");
        }

        return _items.Single(x => x.Index == index);
    }

    /// <summary>
    /// Gets an item slot matching the given slot.
    /// </summary>
    /// <param name="slot">Item slot.</param>
    /// <returns>The item slot.</returns>
    /// <exception cref="InvalidOperationException">The item index is out of range of the container's maximum capacity.</exception>
    public ItemContainerSlot GetAtSlot(int slot)
    {
        if (slot < 0 || slot >= MaxCapacity)
        {
            throw new InvalidOperationException($"Item slot is out of range: '{slot}'");
        }

        return _items[slot];
    }

    /// <summary>
    /// Get a range of slots.
    /// </summary>
    /// <param name="start">Start slot</param>
    /// <param name="count">Number of slots to get.</param>
    /// <returns>A collection of slots.</returns>
    public IEnumerable<ItemContainerSlot> GetRange(int start, int count) => _items.GetRange(start, count);

    /// <summary>
    /// Checks if the given item can be stored in the container.
    /// </summary>
    /// <param name="itemToStore">Item to store.</param>
    /// <returns>True if the item can be stored; false otherwise.</returns>
    public bool CanStoreItem(Item itemToStore)
    {
        if (itemToStore is null)
        {
            return false;
        }

        int quantityToStore = itemToStore.Quantity;
        int itemToStoreMaxQuantity = itemToStore.Properties.PackMax;

        for (int i = 0; i < Capacity; i++)
        {
            ItemContainerSlot itemSlot = GetAtSlot(i);

            if (!itemSlot.HasItem)
            {
                if (quantityToStore > itemToStoreMaxQuantity)
                {
                    quantityToStore -= itemToStoreMaxQuantity;
                }
                else
                {
                    return true;
                }
            }
            else if (itemSlot.Item.Id == itemToStore.Id)
            {
                if (itemSlot.Item.Quantity + quantityToStore > itemToStoreMaxQuantity)
                {
                    quantityToStore -= itemToStoreMaxQuantity - itemSlot.Item.Quantity;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Creates an item inside the container.
    /// </summary>
    /// <param name="item">Item to create.</param>
    /// <returns>Collection of <see cref="ItemCreationResult"/>.</returns>
    public IEnumerable<ItemCreationResult> CreateItem(Item item)
    {
        int quantity = item.Quantity;
        var result = new List<ItemCreationResult>();

        if (!CanStoreItem(item))
        {
            return result;
        }

        if (item.Properties.IsStackable)
        {
            for (int slotIndex = 0; slotIndex < Capacity; slotIndex++)
            {
                ItemContainerSlot slot = GetAtSlot(slotIndex);

                if (slot.HasItem && slot.Item.Id == item.Id && slot.Item.Quantity < slot.Item.Properties.PackMax)
                {
                    if (slot.Item.Quantity + quantity > slot.Item.Properties.PackMax)
                    {
                        quantity -= slot.Item.Properties.PackMax - slot.Item.Quantity;
                        slot.Item.Quantity = slot.Item.Properties.PackMax;
                    }
                    else
                    {
                        slot.Item.Quantity += quantity;
                        quantity = 0;
                    }

                    result.Add(new ItemCreationResult(ItemCreationActionType.Update, slot.Item, slot.Slot, slot.Index));

                    if (quantity == 0)
                    {
                        break;
                    }
                }
            }
        }

        if (quantity > 0)
        {
            for (int slotIndex = 0; slotIndex < Capacity; slotIndex++)
            {
                ItemContainerSlot inventoryItemSlot = GetAtSlot(slotIndex);

                if (!inventoryItemSlot.HasItem)
                {
                    inventoryItemSlot.Item = new Item(item.Properties)
                    {
                        Refine = item.Refine,
                        Element = item.Element,
                        ElementRefine = item.ElementRefine,
                        CreatorId = item.CreatorId
                    };
                    inventoryItemSlot.Slot = slotIndex;

                    if (quantity > inventoryItemSlot.Item.Properties.PackMax)
                    {
                        inventoryItemSlot.Item.Quantity = inventoryItemSlot.Item.Properties.PackMax;
                        quantity -= inventoryItemSlot.Item.Quantity;
                    }
                    else
                    {
                        inventoryItemSlot.Item.Quantity = quantity;
                        quantity = 0;
                    }

                    result.Add(new ItemCreationResult(ItemCreationActionType.Add, inventoryItemSlot.Item, inventoryItemSlot.Slot, inventoryItemSlot.Index));

                    if (quantity == 0)
                    {
                        break;
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Swap two slots.
    /// </summary>
    /// <param name="sourceSlot">Source slot.</param>
    /// <param name="destinationSlot">Destination slot.</param>
    public void SwapItem(int sourceSlot, int destinationSlot)
    {
        if (sourceSlot != -1)
        {
            _items[sourceSlot].Slot = destinationSlot;
        }

        if (destinationSlot != -1)
        {
            _items[destinationSlot].Slot = sourceSlot;
        }

        _items.Swap(sourceSlot, destinationSlot);
    }

    /// <summary>
    /// Serializes the item container to the given packet stream.
    /// </summary>
    /// <param name="packet">Packet stream.</param>
    public void Serialize(FFPacket packet)
    {
        for (int i = 0; i < MaxCapacity; i++)
        {
            packet.WriteInt32(_items[i].Index);
        }

        packet.WriteByte((byte)Count);

        for (int i = 0; i < MaxCapacity; i++)
        {
            ItemContainerSlot itemSlot = _items.ElementAt(i);

            if (itemSlot.HasItem)
            {
                packet.WriteByte((byte)i);
                packet.WriteInt32(itemSlot.Index);
                itemSlot.Item.Serialize(packet);
            }
        }

        for (int i = 0; i < MaxCapacity; i++)
        {
            packet.WriteInt32(_items[i].HasItem ? _items[i].Slot : -1);
        }
    }

    public IEnumerator<ItemContainerSlot> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
}
