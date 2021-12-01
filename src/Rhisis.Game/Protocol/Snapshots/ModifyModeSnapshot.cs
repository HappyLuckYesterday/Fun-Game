using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class ModifyModeSnapshot : FFSnapshot
    {
        public ModifyModeSnapshot(IPlayer player, ModeType mode)
            : base(SnapshotType.MODIFYMODE, player.Id)
        {
            Write((uint)mode);
        }
    }
}
