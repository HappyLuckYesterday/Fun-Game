using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Protocol;
using Sylver.Network.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Features
{
    public class TaskbarContainer<TObject> : ITaskbarContainer<TObject> where TObject : class, IPacketSerializer
    {
        private readonly List<TObject> _objects;

        public int Capacity { get; }

        public int Count => _objects.Count(x => x != null);

        public TaskbarContainer(int capacity)
        {
            _objects = new List<TObject>(new TObject[capacity]);
            Capacity = capacity;
        }

        public bool Add(TObject @object, int slotIndex) => SetAt(slotIndex, @object);

        public bool Remove(int slotIndex) => SetAt(slotIndex, null);

        public void Serialize(INetPacketStream packet)
        {
            packet.Write(Count);

            for (int i = 0; i < Capacity; i++)
            {
                if (_objects[i] != null)
                {
                    _objects[i].Serialize(packet);
                }
            }
        }

        public IEnumerator<TObject> GetEnumerator() => _objects.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _objects.GetEnumerator();

        private bool SetAt(int slotIndex, TObject @object)
        {
            if (slotIndex < 0 || slotIndex >= Capacity)
            {
                return false;
            }

            _objects[slotIndex] = @object;

            return true;
        }
    }
}
