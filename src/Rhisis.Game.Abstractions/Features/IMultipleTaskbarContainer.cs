using Rhisis.Game.Abstractions.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    public interface IMultipleTaskbarContainer<TObject> : IPacketSerializer, IEnumerable<ITaskbarContainer<TObject>> 
        where TObject : class, IPacketSerializer
    {
        int Capacity { get; }

        int Count { get; }

        ITaskbarContainer<TObject> GetContainerAtLevel(int level);
    }
}
