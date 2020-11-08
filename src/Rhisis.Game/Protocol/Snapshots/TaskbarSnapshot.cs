using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
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
