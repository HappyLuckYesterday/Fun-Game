using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;

namespace Rhisis.Game.Protocol.Snapshots
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
