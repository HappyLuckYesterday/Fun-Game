using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots
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
