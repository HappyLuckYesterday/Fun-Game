using Rhisis.Core.Extensions;
using Rhisis.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public class ItemContainer : IEnumerable<ItemContainerSlot>
{
    protected readonly List<ItemContainerSlot> _items;

    public int Count => _items.Count(x => x.HasItem);

    public int Capacity { get; }

    public int ExtraCapacity { get; }

    public int MaxCapacity => Capacity + ExtraCapacity;

    public Item this[int index] => _items[index].Item;

    public ItemContainer(int capacity, int extraCapacity = 0)
    {
        Capacity = capacity;
        ExtraCapacity = extraCapacity;
        _items = Enumerable.Repeat((ItemContainerSlot)null, MaxCapacity).ToList();

        for (int i = 0; i < MaxCapacity; i++)
        {
            _items[i] = new ItemContainerSlot()
            {
                Index = i,
                Slot = i < Capacity ? i : -1,
                Item = null
            };
        }
    }

    public void Initialize(IEnumerable<ItemContainerSlot> items)
    {
        int itemIndex;
        int itemsCount = Math.Min(items.Count(), MaxCapacity);

        for (itemIndex = 0; itemIndex < itemsCount; itemIndex++)
        {
            ItemContainerSlot item = items.ElementAtOrDefault(itemIndex);

            if (item != null)
            {
                int slot = item.Slot;

                _items[slot].Item = item.Item;
                _items[slot].Slot = slot;
            }
        }
    }

    public Item GetItem(int itemId) => _items.FirstOrDefault(x => x.HasItem && x.Item.Id == itemId).Item;

    public ItemContainerSlot GetAtIndex(int index)
    {
        if (index < 0 || index >= MaxCapacity)
        {
            throw new InvalidOperationException($"Item index is out of range: '{index}'");
        }

        return _items.Single(x => x.Index == index);
    }

    public ItemContainerSlot GetAtSlot(int slot)
    {
        if (slot < 0 || slot >= MaxCapacity)
        {
            throw new IndexOutOfRangeException();
        }

        return _items[slot];
    }

    public IEnumerable<ItemContainerSlot> GetRange(int start, int count) => _items.GetRange(start, count);

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

    public void DeleteItem(Item item)
    {
        if (item is null)
        {
            return;
        }

        ItemContainerSlot itemSlot = _items.FirstOrDefault(x => x.HasItem && x.Item.SerialNumber == item.SerialNumber);

        if (itemSlot.Slot >= Capacity)
        {
            //_itemsMask[itemSlot.Slot] = -1;
            //itemSlot.Slot = -1;
            itemSlot.Item = null;
        }
    }

    public void SwapItem(int sourceSlot, int destinationSlot)
    {
        //_itemsMask.Swap(sourceSlot, destinationSlot);

        //int itemSourceIndex = _itemsMask[sourceSlot];
        //int itemDestinationIndex = _itemsMask[destinationSlot];

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
