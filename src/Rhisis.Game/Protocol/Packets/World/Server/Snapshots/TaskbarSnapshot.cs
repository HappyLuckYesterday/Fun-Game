using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class TaskbarSnapshot : FFSnapshot
{
    public TaskbarSnapshot(Player player)
        : base(SnapshotType.TASKBAR, player.ObjectId)
    {
        player.Taskbar.Serialize(this);
    }
}
