using Rhisis.Game.Abstractions.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides an interface to manage a contact list.
    /// </summary>
    public interface IContactList : IPacketSerializer, IEnumerable<IContact>
    {
    }
}
