using Rhisis.Game.Abstractions.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides an interface to manage a contact list.
    /// </summary>
    public interface IContactList : IPacketSerializer, IEnumerable<IContact>
    {
        /// <summary>
        /// Gets the amount of contacts in the current contact list.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the maximum allowed contacts in the current list.
        /// </summary>
        int Maximum { get; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the contact list is full.
        /// </summary>
        bool IsFull => Count >= Maximum;

        /// <summary>
        /// Adds a new contact to the list.
        /// </summary>
        /// <param name="contact">Contact to add.</param>
        void Add(IContact contact);

        /// <summary>
        /// Checks if the given player id is in the contact list.
        /// </summary>
        /// <param name="playerId">Player id.</param>
        /// <returns>True if the player id exists in the contact list; false otherwise.</returns>
        bool Contains(uint playerId);

        /// <summary>
        /// Removes a contact from the list.
        /// </summary>
        /// <param name="contact">Contact to remove.</param>
        void Remove(IContact contact);

        /// <summary>
        /// Gets the contact identified with the given id.
        /// </summary>
        /// <param name="contactId">Contact id.</param>
        /// <returns>Contact if found; null otherwise.</returns>
        IContact Get(int contactId);
    }
}
