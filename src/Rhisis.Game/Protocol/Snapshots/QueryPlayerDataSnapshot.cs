using Rhisis.Game.Abstractions.Caching;
using Rhisis.Protocol;
using System;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class QueryPlayerDataSnapshot : FFSnapshot
    {
        public QueryPlayerDataSnapshot(CachedPlayer player)
            : base(SnapshotType.QUERY_PLAYER_DATA, uint.MaxValue)
        {
            WriteUInt32((uint)player.Id);
            WriteString(player.Name);
            WriteByte((byte)player.Job);
            WriteByte((byte)player.Level);
            WriteByte((byte)player.Gender);
            WriteByte(0);
            WriteInt32(player.Version);
            WriteInt32(Convert.ToInt32(player.IsOnline));
        }
    }
}
