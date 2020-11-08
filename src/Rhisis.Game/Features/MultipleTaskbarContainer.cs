using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Protocol;
using Sylver.Network.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Features
{
    public class MultipleTaskbarContainer<TObject> : IMultipleTaskbarContainer<TObject> where TObject : class, IPacketSerializer
    {
        private readonly List<TaskbarContainer<TObject>> _levels;

        public int Count => _levels.Sum(x => x.Count);

        public int Capacity { get; }

        public MultipleTaskbarContainer(int capacity, int capacityPerLevels)
        {
            _levels = new List<TaskbarContainer<TObject>>(Enumerable.Repeat(new TaskbarContainer<TObject>(capacityPerLevels), capacity));
            Capacity = capacity;
        }

        public ITaskbarContainer<TObject> GetContainerAtLevel(int level)
        {
            if (level < 0 || level >= Capacity)
            {
                return null;
            }

            return _levels[level];
        }

        public void Serialize(INetPacketStream packet)
        {
            packet.Write(Count);

            for (int level = 0; level < Capacity; level++)
            {
                ITaskbarContainer<TObject> taskbarLevel = GetContainerAtLevel(level);

                foreach (TObject shortcut in taskbarLevel)
                {
                    if (shortcut != null)
                    {
                        packet.Write(level);
                        shortcut.Serialize(packet);
                    }
                }
            }
        }

        public IEnumerator<ITaskbarContainer<TObject>> GetEnumerator() => _levels.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => _levels.GetEnumerator();
    }
}
