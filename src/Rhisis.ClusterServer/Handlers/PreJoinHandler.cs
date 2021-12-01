using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.ClusterServer.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.Cluster;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers
{
    [Handler]
    public class PreJoinHandler : ClusterHandlerBase
    {
        private readonly ILogger<PreJoinHandler> _logger;
        private readonly IOptions<ClusterConfiguration> _clusterOptions;
        private readonly IClusterPacketFactory _clusterPacketFactory;

        public PreJoinHandler(ILogger<PreJoinHandler> logger, 
            IOptions<ClusterConfiguration> clusterOptions, 
            IRhisisDatabase database, 
            IClusterPacketFactory clusterPacketFactory)
            : base(database)
        {
            _logger = logger;
            _clusterOptions = clusterOptions;
            _clusterPacketFactory = clusterPacketFactory;
        }

        [HandlerAction(PacketType.PRE_JOIN)]
        public void OnPreJoin(IClusterUser client, PreJoinPacket packet)
        {
            DbCharacter character = Database.Characters.FirstOrDefault(x => x.Id == packet.CharacterId);

            if (character is null)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin character id '{packet.CharacterName}' for user '{packet.Username}'. " +
                      $"Reason: no character with id {packet.CharacterId}.");
                client.Disconnect();
                return;
            }

            if (character.IsDeleted)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin with character '{character.Name}' for user '{packet.Username}'. " +
                    "Reason: character is deleted.");
                client.Disconnect();
                return;
            }

            if (character.Name != packet.CharacterName)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin character '{character.Name}' for user '{packet.Username}'. " +
                    "Reason: character is not owned by this user.");
                client.Disconnect();
                return;
            }

            if (_clusterOptions.Value.EnableLoginProtect &&
                LoginProtect.GetNumPadToPassword(client.LoginProtectValue, packet.BankCode) != character.BankCode)
            {
                _logger.LogWarning($"Unable to prejoin character '{character.Name}' for user '{packet.Username}'. " +
                    "Reason: bad bank code.");
                client.LoginProtectValue = new Random().Next(0, 1000);
                _clusterPacketFactory.SendLoginProtect(client, client.LoginProtectValue);
                return;
            }

            _clusterPacketFactory.SendJoinWorld(client);
            _logger.LogInformation($"Character '{character.Name}' has prejoin successfully the game for user '{packet.Username}'.");
        }
    }
}
