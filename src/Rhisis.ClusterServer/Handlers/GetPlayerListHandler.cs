using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Abstractions.Server;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.Cluster;
using Rhisis.Protocol.Packets.Server.Cluster;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers
{
    /// <summary>
    /// Handles the GETPLAYERLIST packet and sends the user's characters list to the client.
    /// </summary>
    [Handler]
    public class GetPlayerListHandler : ClusterHandlerBase
    {
        private readonly ILogger<GetPlayerListHandler> _logger;
        private readonly IOptions<ClusterOptions> _clusterOptions;
        private readonly IClusterCacheServer _clusterCache;

        /// <summary>
        /// Creates a new <see cref="CharacterHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="database">Rhisis database.</param>
        public GetPlayerListHandler(ILogger<GetPlayerListHandler> logger, 
            IOptions<ClusterOptions> clusterOptions,
            IRhisisDatabase database,
            IClusterCacheServer clusterCache)
            : base(database)
        {
            _logger = logger;
            _clusterOptions = clusterOptions;
            _clusterCache = clusterCache;
        }

        [HandlerAction(PacketType.GETPLAYERLIST)]
        public void Execute(IClusterUser user, GetPlayerListPacket packet)
        {
            WorldChannel selectedWorldServer = _clusterCache.WorldChannels.SingleOrDefault(x => x.Id == packet.ServerId);

            if (selectedWorldServer is null)
            {
                _logger.LogWarning($"Unable to get characters list for user '{packet.Username}'. " +
                    "Reason: client requested the list on a not connected World server.");
                user.Disconnect();
                return;
            }

            DbUser dbUser = Database.Users.FirstOrDefault(x => x.Username == packet.Username);

            if (dbUser is null)
            {
                _logger.LogWarning($"[SECURITY] Unable to load character list for user '{packet.Username}'. " +
                    "Reason: bad presented credentials compared to the database.");
                user.Disconnect();
                return;
            }

            user.UserId = dbUser.Id;
            user.Username = dbUser.Username;

            SendPlayerList(user, packet.AuthenticationKey);

            using var cacheAddressPacket = new CacheAddressPacket(selectedWorldServer.Host);
            user.Send(cacheAddressPacket);


            if (_clusterOptions.Value.EnableLoginProtect)
            {
                using var loginNumPadPacket = new LoginProctectNumPadPacket(user.LoginProtectValue);
                user.Send(loginNumPadPacket);
            }
        }
    }
}
