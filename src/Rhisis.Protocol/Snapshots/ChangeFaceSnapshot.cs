using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class ChangeFaceSnapshot : FFSnapshot
{
    public ChangeFaceSnapshot(IPlayer player, uint faceId)
        : base(SnapshotType.CHANGEFACE, player.Id)
    {
        WriteInt32(player.CharacterId);
        WriteUInt32(faceId);
    }
}
