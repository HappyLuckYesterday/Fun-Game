using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Messages
{
    [Handler]
    public class PlayerCacheUpdateMessageHandler
    {
        private readonly IWorldServer _worldServer;
        private readonly IPlayerCache _playerCache;

        public PlayerCacheUpdateMessageHandler(IWorldServer worldServer, IPlayerCache playerCache)
        {
            _worldServer = worldServer;
            _playerCache = playerCache;
        }

        [HandlerAction(typeof(PlayerCacheUpdate))]
        public void OnExecute(PlayerCacheUpdate message)
        {
            CachedPlayer cachedPlayer = _playerCache.GetCachedPlayer(message.PlayerId);

            if (cachedPlayer is null)
            {
                throw new InvalidOperationException($"Failed to retrieve cached player with id: '{message.PlayerId}'.");
            }

            using var playerDataSnapshot = new QueryPlayerDataSnapshot(cachedPlayer);

            _worldServer.SendToAll(playerDataSnapshot);
        }
    }
}
