using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Client;
using Rhisis.ClusterServer.Packets;
using Rhisis.ClusterServer.Structures;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions.Caching;
using Rhisis.Network;
using Rhisis.Network.Core.Servers;
using Rhisis.Network.Packets.Cluster;
using Sylver.HandlerInvoker.Attributes;
using System.Collections.Generic;
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
        private readonly IClusterServer _clusterServer;
        private readonly IClusterPacketFactory _clusterPacketFactory;
        private readonly IRhisisCacheManager _cacheManager;

        /// <summary>
        /// Creates a new <see cref="CharacterHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="database">Rhisis database.</param>
        /// <param name="clusterServer">Cluster server instance.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="clusterPacketFactory">Cluster server packet factory.</param>
        public GetPlayerListHandler(ILogger<GetPlayerListHandler> logger, IRhisisDatabase database, IClusterServer clusterServer,
            IClusterPacketFactory clusterPacketFactory, IRhisisCacheManager cacheManager)
            : base(database)
        {
            _logger = logger;
            _clusterServer = clusterServer;
            _clusterPacketFactory = clusterPacketFactory;
            _cacheManager = cacheManager;
        }

        [HandlerAction(PacketType.GETPLAYERLIST)]
        public void Execute(IClusterClient client, GetPlayerListPacket packet)
        {
            var selectedWorldServer = _cacheManager.GetCache(CacheType.ClusterWorldChannels).Get<WorldChannel>(packet.ServerId.ToString());

            if (selectedWorldServer is null)
            {
                _logger.LogWarning($"Unable to get characters list for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                    "Reason: client requested the list on a not connected World server.");
                client.Disconnect();
                return;
            }

            DbUser dbUser = Database.Users.FirstOrDefault(x => x.Username == packet.Username);

            if (dbUser is null)
            {
                _logger.LogWarning($"[SECURITY] Unable to load character list for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                    "Reason: bad presented credentials compared to the database.");
                client.Disconnect();
                return;
            }

            client.UserId = dbUser.Id;
            client.Username = dbUser.Username;

            IEnumerable<ClusterCharacter> characters = GetCharacters(dbUser.Id);

            _clusterPacketFactory.SendPlayerList(client, packet.AuthenticationKey, characters);
            _clusterPacketFactory.SendWorldAddress(client, selectedWorldServer.Host);

            if (_clusterServer.ClusterConfiguration.EnableLoginProtect)
            {
                _clusterPacketFactory.SendLoginNumPad(client, client.LoginProtectValue);
            }
        }
    }
}
