using System.Collections.Generic;
using System.Linq;
using Rhisis.Protocol;

namespace Rhisis.Game.TaskbarPlayer;

public class MultipleTaskbarContainer<TObject> : IPacketSerializer where TObject : class, IPacketSerializer
{
    private readonly List<TaskbarContainer<TObject>> _levels;

    public int Count => _levels.Sum(x => x.Count);

    public int Capacity { get; }

    public MultipleTaskbarContainer(int capacity, int capacityPerLevels)
    {
        _levels = new List<TaskbarContainer<TObject>>(Enumerable.Repeat(new TaskbarContainer<TObject>(capacityPerLevels), capacity));
        Capacity = capacity;
    }

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
                if (shortcut != null)
                {
                    packet.WriteInt32(level);
                    shortcut.Serialize(packet);
                }
            }
        }
    }
}