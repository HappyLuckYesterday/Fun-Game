using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class QueryPlayerDataHandler
    {
        private readonly IPlayerCache _playerCache;

        public QueryPlayerDataHandler(IPlayerCache playerCache)
        {
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
                    cachedPlayer = _playerCache.LoadCachedPlayer((int)packet.PlayerId);
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
                        cachedPlayer.Version = packet.Version + 1;
                        cachedPlayer.IsOnline = true;
                        cachedPlayer.MessengerStatus = player.Messenger.Status;

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
    }
}
