using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides an interface to manage a mover's buffs.
    /// </summary>
    public interface IBuffs : IPacketSerializer, IEnumerable<IBuff>
    {
        /// <summary>
        /// Adds a buff.
        /// </summary>
        /// <param name="buff">Buff to add.</param>
        /// <returns>The buff result type.</returns>
        BuffResultType Add(IBuff buff);

        /// <summary>
        /// Removes a buff.
        /// </summary>
        /// <param name="buff">Buff to remove.</param>
        /// <returns>True if the buff has been removed; false otherwise.</returns>
        bool Remove(IBuff buff);

        /// <summary>
        /// Removes all the buffs.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Check if the given buff exists in the buffs list.
        /// </summary>
        /// <param name="buff">Buff.</param>
        /// <returns>True if the buff exists; false otherwise.</returns>
        bool Contains(IBuff buff);

        /// <summary>
        /// Update the buff timers.
        /// </summary>
        void Update();
    }
}
