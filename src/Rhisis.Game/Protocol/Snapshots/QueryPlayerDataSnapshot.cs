using Rhisis.Game.Abstractions.Caching;
using System;

namespace Rhisis.Network.Snapshots
{
    public class QueryPlayerDataSnapshot : FFSnapshot
    {
        public QueryPlayerDataSnapshot(CachedPlayer player)
            : base(SnapshotType.QUERY_PLAYER_DATA, uint.MaxValue)
        {
            WriteUInt32((uint)player.Id);
            WriteString(player.Name);
            WriteInt32((int)player.Job);
            WriteInt32(player.Level);
            WriteInt32((int)player.Gender);
            WriteInt32(player.Version);
            WriteInt32(Convert.ToInt32(player.IsOnline));
        }
    }
}
