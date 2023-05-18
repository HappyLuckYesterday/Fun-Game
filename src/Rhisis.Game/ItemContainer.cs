using Rhisis.Core.Extensions;
using Rhisis.Game.Common;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public class ItemContainer
{
    protected readonly ItemContainerSlot[] _items;
    protected readonly int[] _slots;

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
        _items = new ItemContainerSlot[MaxCapacity];
        _slots = new int[MaxCapacity];

        for (int i = 0; i < MaxCapacity; i++)
        {
            _items[i] = new()
            {
                Index = i,
                Item = null
            };

            if (i < Capacity)
            {
                _items[i].Number = i;
                _slots[i] = i;
            }
            else
            {
                _slots[i] = -1;
            }
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
                _items[itemSlot].Number = itemSlot;
                _items[itemSlot].Index = itemSlot;
                _slots[itemSlot] = itemSlot;
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

        return _items[index];
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

        int itemIndex = _slots[slot];

        if (itemIndex < 0 || itemIndex >= MaxCapacity)
        {
            return ItemContainerSlot.Empty;
        }

        return _items[itemIndex];
    }

    public ItemContainerSlot FindSlot(Func<ItemContainerSlot, bool> predicate) => _items.FirstOrDefault(predicate);

    /// <summary>
    /// Get a range of slots.
    /// </summary>
    /// <param name="start">Start slot</param>
    /// <param name="count">Number of slots to get.</param>
    /// <returns>A collection of slots.</returns>
    public IEnumerable<ItemContainerSlot> GetRange(int start, int count) => _slots.GetRange(start, count).Select(x => x > 0 ? _items[x] : ItemContainerSlot.Empty);

    /// <summary>
    /// Gets the number of available slots in the storage container.
    /// </summary>
    /// <returns>Number of available slots.</returns>
    public int GetStorageCount() => GetRange(0, Capacity).Count(x => !x.HasItem);

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
            for (int i = 0; i < Capacity; i++)
            {
                int index = _slots[i];

                if (index < 0 || index >= MaxCapacity)
                {
                    continue;
                }

                ItemContainerSlot slot = _items[index];

                if (slot.HasItem && slot.Item.Id == item.Id && item.Quantity < item.Properties.PackMax)
                {
                    if (slot.Item.Quantity + quantity > item.Properties.PackMax)
                    {
                        quantity -= item.Properties.PackMax - slot.Item.Quantity;
                        slot.Item.Quantity = item.Properties.PackMax;
                    }
                    else
                    {
                        slot.Item.Quantity += quantity;
                        quantity = 0;
                    }

                    result.Add(new ItemCreationResult(ItemCreationActionType.Update, slot.Item, slot.Number, slot.Index));

                    if (quantity == 0)
                    {
                        break;
                    }
                }
            }
        }

        if (quantity > 0)
        {
            for (int i = 0; i < Capacity; i++)
            {
                int index = _slots[i];

                if (index < 0 || index >= MaxCapacity)
                {
                    continue;
                }

                ItemContainerSlot slot = _items[index];

                if (!slot.HasItem)
                {
                    slot.Index = index;
                    slot.Number = i;
                    slot.Item = new Item(item.Properties)
                    {
                        Refine = item.Refine,
                        Element = item.Element,
                        ElementRefine = item.ElementRefine,
                        CreatorId = item.CreatorId
                    };

                    if (quantity > slot.Item.Properties.PackMax)
                    {
                        slot.Item.Quantity = slot.Item.Properties.PackMax;
                        quantity -= slot.Item.Quantity;
                    }
                    else
                    {
                        slot.Item.Quantity = quantity;
                        quantity = 0;
                    }

                    result.Add(new ItemCreationResult(ItemCreationActionType.Add, slot.Item, slot.Number, slot.Index));

                    if (quantity == 0)
                    {
                        break;
                    }
                }
            }
        }

        return result;
    }

    public void Remove(ItemContainerSlot itemSlot)
    {
        if (!itemSlot.HasItem || itemSlot.Index >= MaxCapacity || itemSlot.Number >= MaxCapacity)
        {
            return;
        }

        itemSlot.Item = null;
        if (itemSlot.Number >= Capacity)
        {
            _slots[itemSlot.Number] = - 1;
            itemSlot.Number = -1;
        }
    }

    /// <summary>
    /// Swap two slots.
    /// </summary>
    /// <param name="sourceSlot">Source slot.</param>
    /// <param name="destinationSlot">Destination slot.</param>
    protected void SwapItem(int sourceSlot, int destinationSlot)
    {
        _slots.Swap(sourceSlot, destinationSlot);

        int sourceIndex = _slots[sourceSlot];
        int destinationIndex = _slots[destinationSlot];
        
        if (sourceIndex != -1)
        {
            _items[sourceIndex].Number = sourceSlot;
        }

        if (destinationIndex != -1)
        {
            _items[destinationIndex].Number = destinationSlot;
        }
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
            packet.WriteInt32(_items[i].Number);
        }
    }
}
