using Rhisis.Game.Abstractions.Protocol;
using Rhisis.World.Game.Structures;
using Sylver.Network.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class BuffComponent : IEnumerable<Buff>, IPacketSerializer
    {
        private readonly List<Buff> _buffs;

        public BuffComponent()
        {
            _buffs = new List<Buff>();
        }

        public void Add(Buff buff)
        {
            _buffs.Add(buff);
        }

        public void Remove(Buff buff)
        {
            _buffs.Remove(buff);
        }

        public void Clear()
        {
            _buffs.Clear();
        }

        public IEnumerator<Buff> GetEnumerator() => _buffs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _buffs.GetEnumerator();

        public void Serialize(INetPacketStream packet)
        {
            IEnumerable<Buff> activeBuffs = _buffs.Where(x => !x.HasExpired);

            packet.Write(activeBuffs.Count());

            foreach (Buff buff in activeBuffs)
            {
                buff.Serialize(packet);
            }
        }
    }
}
