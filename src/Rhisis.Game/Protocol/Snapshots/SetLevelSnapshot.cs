using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
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
