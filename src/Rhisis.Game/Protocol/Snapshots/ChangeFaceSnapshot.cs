using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
{
    public class ChangeFaceSnapshot : FFSnapshot
    {
        public ChangeFaceSnapshot(IPlayer player, uint faceId)
            : base(SnapshotType.CHANGEFACE, player.Id)
        {
            Write(player.CharacterId);
            Write(faceId);
        }
    }
}
