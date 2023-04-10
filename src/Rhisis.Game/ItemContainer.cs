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
    protected readonly int[] _itemsMask;

    public int Count => _items.Count(x => x.HasItem);

    public int Capacity { get; }

    public int ExtraCapacity { get; }

    public int MaxCapacity => Capacity + ExtraCapacity;

    public int[] Masks => _itemsMask;

    public Item this[int index] => _items[index].Item;

    public ItemContainer(int capacity, int extraCapacity = 0)
    {
        Capacity = capacity;
        ExtraCapacity = extraCapacity;
        _itemsMask = new int[MaxCapacity];
        _items = Enumerable.Repeat((ItemContainerSlot)null, MaxCapacity).ToList();

        for (int i = 0; i < MaxCapacity; i++)
        {
            _itemsMask[i] = i < Capacity ? i : -1;
            _items[i] = new ItemContainerSlot()
            {
                Index = i,
                Slot = i < Capacity ? i : 0,
                Item = null
            };
            //_items[i].Index = i;

            //if (i < Capacity)
            //{
            //    _items[i].Slot = i;
            //    _itemsMask[i] = i;
            //}
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
                _items[slot].Index = slot;
                _itemsMask[slot] = slot;
            }
        }
    }

    public Item GetItem(int itemId) => _items.FirstOrDefault(x => x.HasItem && x.Item.Id == itemId).Item;

    public ItemContainerSlot GetAtIndex(int index)
    {
        if (index < 0 || index >= MaxCapacity)
        {
            return ItemContainerSlot.Empty;
        }

        return _items[index];
    }

    public ItemContainerSlot GetAtSlot(int slot)
    {
        if (slot < 0 || slot >= MaxCapacity)
        {
            throw new IndexOutOfRangeException();
        }

        return GetAtIndex(_itemsMask[slot]);
    }

    public IEnumerable<ItemContainerSlot> GetRange(int start, int count) => _itemsMask.GetRange(start, count).Select(GetAtIndex);

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
            for (int i = 0; i < Capacity; i++)
            {
                int itemIndex = _itemsMask[i];

                if (itemIndex < 0 || itemIndex >= MaxCapacity)
                {
                    continue;
                }

                Item inventoryItem = _items[itemIndex].Item;

                if (inventoryItem is not null && inventoryItem.Id == item.Id && inventoryItem.Quantity < inventoryItem.Properties.PackMax)
                {
                    if (inventoryItem.Quantity + quantity > inventoryItem.Properties.PackMax)
                    {
                        quantity -= inventoryItem.Properties.PackMax - inventoryItem.Quantity;
                        inventoryItem.Quantity = inventoryItem.Properties.PackMax;
                    }
                    else
                    {
                        inventoryItem.Quantity += quantity;
                        quantity = 0;
                    }

                    result.Add(new ItemCreationResult(ItemCreationActionType.Update, inventoryItem));

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
                int itemIndex = _itemsMask[i];

                if (itemIndex < 0 || itemIndex >= MaxCapacity)
                {
                    continue;
                }

                ItemContainerSlot inventoryItemSlot = _items[itemIndex];

                if (!inventoryItemSlot.HasItem)
                {
                    inventoryItemSlot.Item = new Item(item.Properties)
                    {
                        Refine = item.Refine,
                        Element = item.Element,
                        ElementRefine = item.ElementRefine,
                        CreatorId = item.CreatorId
                    };
                    inventoryItemSlot.Index = itemIndex;
                    inventoryItemSlot.Slot = i;

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

                    result.Add(new ItemCreationResult(ItemCreationActionType.Add, inventoryItemSlot.Item));

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
            _itemsMask[itemSlot.Slot] = -1;
            itemSlot.Slot = -1;
            itemSlot.Item = null;
        }
    }

    public void SwapItem(int sourceSlot, int destinationSlot)
    {
        _itemsMask.Swap(sourceSlot, destinationSlot);

        int itemSourceIndex = _itemsMask[sourceSlot];
        int itemDestinationIndex = _itemsMask[destinationSlot];

        if (itemSourceIndex != -1)
        {
            _items[itemSourceIndex].Slot = sourceSlot;
        }

        if (itemDestinationIndex != -1)
        {
            _items[itemDestinationIndex].Slot = destinationSlot;
        }
    }

    public void Serialize(FFPacket packet)
    {
        for (int i = 0; i < MaxCapacity; i++)
        {
            packet.WriteInt32(_itemsMask[i]);
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
