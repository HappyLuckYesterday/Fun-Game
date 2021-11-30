using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class SetLevelSnapshot : FFSnapshot
    {
        public SetLevelSnapshot(IPlayer player, int level)
            : base(SnapshotType.SETLEVEL, player.Id)
        {
            Write((short)level);
        }
    }
}
