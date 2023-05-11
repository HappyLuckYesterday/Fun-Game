using Rhisis.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.TaskbarPlayer;

/// <summary>
/// Provides a mechanism to manage a multi-level taskbar.
/// </summary>
/// <typeparam name="TObject">Object type that implements the <see cref="IPacketSerializer"/> interface.</typeparam>
public class MultipleTaskbarContainer<TObject> : IEnumerable<TaskbarContainer<TObject>>, IPacketSerializer 
    where TObject : class, IPacketSerializer
{
    private readonly TaskbarContainer<TObject>[] _levels;

    /// <summary>
    /// Gets the total amount of sub levels.
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Gets the total amount objects in this taskbar. Including the sub levels.
    /// </summary>
    /// <remarks>
    /// Sum of all count of sub levels.
    /// </remarks>
    public int Count => _levels.Sum(x => x.Count);

    public MultipleTaskbarContainer(int capacity, int capacityPerLevels)
    {
        _levels = new TaskbarContainer<TObject>[capacity];

        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i] = new TaskbarContainer<TObject>(capacityPerLevels);
        }
        
        Capacity = capacity;
    }

    /// <summary>
    /// Gets the taskbar container at a given level.
    /// </summary>
    /// <param name="level">Taskbar level.</param>
    /// <returns>The taskbar if found; null if level is out of range.</returns>
    public TaskbarContainer<TObject> GetContainerAtLevel(int level)
    {
        if (level < 0 || level >= Capacity)
        {
            return null;
        }

        return _levels[level];
    }

    public void Serialize(FFPacket packet)
    {
        packet.WriteInt32(Count);

        for (int level = 0; level < Capacity; level++)
        {
            TaskbarContainer<TObject> taskbarLevel = GetContainerAtLevel(level);

            foreach (TObject shortcut in taskbarLevel)
            {
                if (shortcut is not null)
                {
                    packet.WriteInt32(level);
                    shortcut.Serialize(packet);
                }
            }
        }
    }

    public IEnumerator<TaskbarContainer<TObject>> GetEnumerator() => (IEnumerator<TaskbarContainer<TObject>>)_levels.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _levels.GetEnumerator();
}