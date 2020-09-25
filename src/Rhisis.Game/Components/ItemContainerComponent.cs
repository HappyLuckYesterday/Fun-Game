using Rhisis.Core.Extensions;
using Rhisis.Game.Abstractions.Entities;
using Sylver.Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Abstractions.Components
{
    public class ItemContainerComponent<TItem> : IItemContainer
        where TItem : class, IItem
    {
        protected readonly List<IItem> _items;
        protected readonly int[] _itemsMask;

        public int Count => _items.Count(x => x != null && x.Id != -1);

        public int Capacity { get; }

        public int ExtraCapacity { get; }

        public int MaxCapacity => Capacity + ExtraCapacity;

        public int[] Masks => _itemsMask;

        public IItem this[int index] => _items[index];

        public ItemContainerComponent(int capacity, int extraCapacity = 0)
        {
            Capacity = capacity;
            ExtraCapacity = extraCapacity;
            _itemsMask = new int[MaxCapacity];
            _items = Enumerable.Repeat((IItem)null, MaxCapacity).ToList();

            for (int i = 0; i < MaxCapacity; i++)
            {
                _itemsMask[i] = -1;
                _items[i] = Activator.CreateInstance<TItem>();
                _items[i].Index = i;

                if (i < Capacity)
                {
                    _items[i].Slot = i;
                    _itemsMask[i] = i;
                }
            }
        }

        public void Initialize(IEnumerable<IItem> items)
        {
            int itemIndex;
            int itemsCount = Math.Min(items.Count(), MaxCapacity);

            for (itemIndex = 0; itemIndex < itemsCount; itemIndex++)
            {
                IItem item = items.ElementAtOrDefault(itemIndex);

                if (item != null)
                {
                    int slot = item.Slot;

                    _items[slot].CopyFrom(item);
                    _items[slot].Slot = slot;
                    _items[slot].Index = slot;
                    _itemsMask[slot] = slot;
                }
            }
        }

        public void CreateItem(IItem item)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(IItem item)
        {
            if (item == null || item.Slot >= MaxCapacity)
            {
                return;
            }

            item.Reset();

            if (item.Slot >= Capacity)
            {
                _itemsMask[item.Slot] = -1;
                item.Slot = -1;
            }
        }

        public IItem GetItem(Func<IItem, bool> predicate)
        {
            IItem item = _items.FirstOrDefault(predicate);

            if (item == null)
            {
                return null;
            }

            return item.Id != -1 ? item : null;
        }

        public IItem GetItem(int itemId) => GetItem(x => x.Id == itemId);

        public IItem GetItemAtIndex(int index)
        {
            if (index < 0 || index >= MaxCapacity)
            {
                return null;
            }

            IItem item = _items[index];

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), $"No item found at index {index}.");
            }

            return item.Id != -1 ? item : null;
        }

        public IItem GetItemAtSlot(int slot)
        {
            if (slot < 0 || slot >= MaxCapacity)
            {
                throw new IndexOutOfRangeException();
            }

            return GetItemAtIndex(_itemsMask[slot]);
        }

        public IEnumerable<IItem> GetRange(int start, int count)
        {
            return _itemsMask.GetRange(start, count).Select(index => GetItemAtIndex(index));
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

        public void Serialize(INetPacketStream packet)
        {
            for (int i = 0; i < MaxCapacity; i++)
            {
                packet.Write(_itemsMask[i]);
            }

            packet.Write((byte)Count);

            for (int i = 0; i < MaxCapacity; i++)
            {
                IItem item = _items.ElementAt(i);

                if (item != null && item.Id != -1)
                {
                    packet.Write((byte)i);
                    item.Serialize(packet);
                }
            }

            for (int i = 0; i < MaxCapacity; i++)
            {
                packet.Write(_items[i]?.Slot ?? -1);
            }
        }

        public IEnumerator<IItem> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
