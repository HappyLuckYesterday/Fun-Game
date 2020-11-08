using Rhisis.Game.Abstractions.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    public interface ITaskbarContainer<TObject> : IPacketSerializer, IEnumerable<TObject>
        where TObject : class, IPacketSerializer
    {
        int Capacity { get; }

        int Count { get; }

        bool Add(TObject @object, int slotIndex);

        bool Remove(int slotIndex);
    }
}
