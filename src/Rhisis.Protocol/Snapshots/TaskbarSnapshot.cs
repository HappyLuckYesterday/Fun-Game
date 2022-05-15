using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class TaskbarSnapshot : FFSnapshot
    {
        public TaskbarSnapshot(IPlayer player)
            : base(SnapshotType.TASKBAR, player.Id)
        {
            player.Taskbar.Serialize(this);
        }
    }
}
