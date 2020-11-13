using Rhisis.Game.Abstractions.Protocol;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage a multi-level taskbar.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public interface IMultipleTaskbarContainer<TObject> : IPacketSerializer, IEnumerable<ITaskbarContainer<TObject>> 
        where TObject : class, IPacketSerializer
    {
        /// <summary>
        /// Gets the total amount of sub levels.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Gets the total amount objects in this taskbar. Including the sub levels.
        /// </summary>
        /// <remarks>
        /// Sum of all count of sub levels.
        /// </remarks>
        int Count { get; }

        /// <summary>
        /// Gets the taskbar container at a given level.
        /// </summary>
        /// <param name="level">Taskbar level.</param>
        /// <returns>The taskbar if found; throws a <see cref="IndexOutOfRangeException"/> exception if level is out of range.</returns>
        /// <exception cref="IndexOutOfRangeException">Throw when the level is out of range.</exception>
        ITaskbarContainer<TObject> GetContainerAtLevel(int level);
    }
}
