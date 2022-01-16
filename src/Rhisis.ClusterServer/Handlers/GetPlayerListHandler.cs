using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.ClusterServer.Packets;
using Rhisis.ClusterServer.Structures;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Abstractions.Caching;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Core.Servers;
using Rhisis.Protocol.Packets.Client.Cluster;
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
        private readonly IOptions<ClusterConfiguration> _clusterOptions;
        private readonly IClusterPacketFactory _clusterPacketFactory;
        private readonly IRhisisCacheManager _cacheManager;

        /// <summary>
        /// Creates a new <see cref="CharacterHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="database">Rhisis database.</param>
        /// <param name="clusterPacketFactory">Cluster server packet factory.</param>
        /// <param name="cacheManager">Cache manager.</param>
        public GetPlayerListHandler(ILogger<GetPlayerListHandler> logger, 
            IOptions<ClusterConfiguration> clusterOptions,
            IRhisisDatabase database,
            IClusterPacketFactory clusterPacketFactory, 
            IRhisisCacheManager cacheManager)
            : base(database)
        {
            _logger = logger;
            _clusterOptions = clusterOptions;
            _clusterPacketFactory = clusterPacketFactory;
            _cacheManager = cacheManager;
        }

        [HandlerAction(PacketType.GETPLAYERLIST)]
        public void Execute(IClusterUser client, GetPlayerListPacket packet)
        {
            var selectedWorldServer = _cacheManager.GetCache(CacheType.ClusterWorldChannels).Get<WorldChannel>(packet.ServerId.ToString());

            if (selectedWorldServer is null)
            {
                _logger.LogWarning($"Unable to get characters list for user '{packet.Username}'. " +
                    "Reason: client requested the list on a not connected World server.");
                client.Disconnect();
                return;
            }

            DbUser dbUser = Database.Users.FirstOrDefault(x => x.Username == packet.Username);

            if (dbUser is null)
            {
                _logger.LogWarning($"[SECURITY] Unable to load character list for user '{packet.Username}'. " +
                    "Reason: bad presented credentials compared to the database.");
                client.Disconnect();
                return;
            }

            client.UserId = dbUser.Id;
            client.Username = dbUser.Username;

            IEnumerable<ClusterCharacter> characters = GetCharacters(dbUser.Id);

            _clusterPacketFactory.SendPlayerList(client, packet.AuthenticationKey, characters);
            _clusterPacketFactory.SendWorldAddress(client, selectedWorldServer.Host);

            if (_clusterOptions.Value.EnableLoginProtect)
            {
                _clusterPacketFactory.SendLoginNumPad(client, client.LoginProtectValue);
            }
        }
    }
}
