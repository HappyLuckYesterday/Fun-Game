using Rhisis.Game.Abstractions.Features;
using Sylver.Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Abstractions.Components
{
    public class MessengerContactList : GameFeature, IContactList
    {
        private readonly List<IContact> _contacts;

        public int Count => _contacts.Count;

        public int Maximum { get; }

        public MessengerContactList(int maximum)
        {
            _contacts = new List<IContact>(maximum);
            Maximum = maximum;
        }

        public void Add(IContact contact)
        {
            if (_contacts.Contains(contact))
            {
                throw new InvalidOperationException("Cannot add the same contact twice.");
            }

            _contacts.Add(contact);
        }

        public bool Contains(uint playerId) => _contacts.Any(x => x.Id == playerId);

        public void Remove(IContact contact)
        {
            _contacts.Remove(contact);
        }

        public void Serialize(INetPacketStream packet)
        {
            packet.WriteInt32(Count);

            foreach (IContact contact in _contacts)
            {
                packet.WriteInt32(contact.Id);
                packet.WriteInt32(Convert.ToInt32(contact.IsBlocked));
                packet.WriteInt32(Convert.ToInt32(contact.Status));
            }
        }

        public IEnumerator<IContact> GetEnumerator() => _contacts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _contacts.GetEnumerator();
    }
}
