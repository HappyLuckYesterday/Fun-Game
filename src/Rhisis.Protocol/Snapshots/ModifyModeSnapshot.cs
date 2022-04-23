using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots
{
    public class ModifyModeSnapshot : FFSnapshot
    {
        public ModifyModeSnapshot(IPlayer player, ModeType mode)
            : base(SnapshotType.MODIFYMODE, player.Id)
        {
            WriteUInt32((uint)mode);
        }
    }
}
