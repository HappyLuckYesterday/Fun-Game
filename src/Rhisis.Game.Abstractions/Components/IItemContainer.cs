using Rhisis.Game.Abstractions.Protocol;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Components
{
    public interface IItemContainer : IPacketSerializer, IEnumerable<IItem>
    {
        int[] Masks { get; }

        int Count { get; }

        int Capacity { get; }

        int ExtraCapacity { get; }

        int MaxCapacity { get; }

        void Initialize(IEnumerable<IItem> items);

        IItem GetItem(Func<IItem, bool> predicate);

        IItem GetItem(int itemId);

        IItem GetItemAtSlot(int slot);

        IItem GetItemAtIndex(int index);

        void CreateItem(IItem item);

        void DeleteItem(IItem item);
    }
}
