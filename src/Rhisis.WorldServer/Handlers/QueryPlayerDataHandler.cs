using Rhisis.Database;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class QueryPlayerDataHandler
    {
        private readonly IWorldServer _server;
        private readonly IRhisisDatabase _database;
        private readonly IPlayerCache _playerCache;

        public QueryPlayerDataHandler(IWorldServer server, IRhisisDatabase database, IPlayerCache playerCache)
        {
            _server = server;
            _database = database;
            _playerCache = playerCache;
        }

        [HandlerAction(PacketType.QUERY_PLAYER_DATA)]
        public void OnExecute(IPlayer player, QueryPlayerDataPacket packet)
        {
            var cachedPlayer = _playerCache.GetCachedPlayer((int)packet.PlayerId);

            if (cachedPlayer is null)
            {
                if (player.CharacterId != packet.PlayerId)
                {
                    cachedPlayer = GetCachedPlayerFromDatabase((int)packet.PlayerId);

                    if (cachedPlayer is null)
                    {
                        throw new InvalidOperationException($"Failed to fetch player with id: {packet.PlayerId}");
                    }

                    _playerCache.SetCachedPlayer(cachedPlayer);
                }
            }
            else
            {
                if (cachedPlayer.Version != packet.Version)
                {
                    if (player.CharacterId == packet.PlayerId)
                    {
                        cachedPlayer.Job = player.Job.Id;
                        cachedPlayer.Level = player.Level;
                        cachedPlayer.Version++;
                        cachedPlayer.IsOnline = true;

                        _playerCache.SetCachedPlayer(cachedPlayer);
                    }
                }
            }

            if (cachedPlayer != null)
            {
                using var queryPlayerDataSnapshot = new QueryPlayerDataSnapshot(cachedPlayer);
                player.Send(queryPlayerDataSnapshot);
            }
        }

        private CachedPlayer GetCachedPlayerFromDatabase(int playerId)
        {
            return _database.Characters
                .Where(x => x.Id == playerId)
                .Select(x => new CachedPlayer(x.Id, -1, x.Name, (GenderType)x.Gender)
                {
                    Job = (DefineJob.Job)x.JobId,
                    Level = x.Level
                })
                .FirstOrDefault();
        }
    }
}
