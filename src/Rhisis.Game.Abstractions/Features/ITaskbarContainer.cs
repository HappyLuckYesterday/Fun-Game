using Rhisis.Game.Abstractions.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage a taskbar.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface ITaskbarContainer<TObject> : IPacketSerializer, IEnumerable<TObject>
        where TObject : class, IPacketSerializer
    {
        /// <summary>
        /// Gets the taskbar capacity.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Get the number of objects in the taskbar.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds an object at a given slot.
        /// </summary>
        /// <param name="object">Object to add.</param>
        /// <param name="slotIndex">Slot.</param>
        /// <returns>True if the object is added; false otherwise.</returns>
        bool Add(TObject @object, int slotIndex);

        /// <summary>
        /// Removes an object at the given slot.
        /// </summary>
        /// <param name="slotIndex">Slot index.</param>
        /// <returns>True if the object has been removed; false otherwise.</returns>
        bool Remove(int slotIndex);
    }
}
