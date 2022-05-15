using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class SetLevelSnapshot : FFSnapshot
    {
        public SetLevelSnapshot(IPlayer player, int level)
            : base(SnapshotType.SETLEVEL, player.Id)
        {
            WriteInt16((short)level);
        }
    }
}
