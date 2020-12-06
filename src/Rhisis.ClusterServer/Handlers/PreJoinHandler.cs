using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Client;
using Rhisis.ClusterServer.Packets;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.Cluster;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers
{
    [Handler]
    public class PreJoinHandler : ClusterHandlerBase
    {
        private readonly ILogger<PreJoinHandler> _logger;
        private readonly IClusterServer _clusterServer;
        private readonly IClusterPacketFactory _clusterPacketFactory;

        public PreJoinHandler(ILogger<PreJoinHandler> logger, IRhisisDatabase database, IClusterServer clusterServer, IClusterPacketFactory clusterPacketFactory)
            : base(database)
        {
            _logger = logger;
            _clusterServer = clusterServer;
            _clusterPacketFactory = clusterPacketFactory;
        }

        [HandlerAction(PacketType.PRE_JOIN)]
        public void OnPreJoin(IClusterClient client, PreJoinPacket packet)
        {
            DbCharacter character = Database.Characters.FirstOrDefault(x => x.Id == packet.CharacterId);

            if (character is null)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin character id '{packet.CharacterName}' for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                      $"Reason: no character with id {packet.CharacterId}.");
                client.Disconnect();
                return;
            }

            if (character.IsDeleted)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin with character '{character.Name}' for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                                "Reason: character is deleted.");
                client.Disconnect();
                return;
            }

            if (character.Name != packet.CharacterName)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin character '{character.Name}' for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                       "Reason: character is not owned by this user.");
                client.Disconnect();
                return;
            }

            if (_clusterServer.ClusterConfiguration.EnableLoginProtect &&
                LoginProtect.GetNumPadToPassword(client.LoginProtectValue, packet.BankCode) != character.BankCode)
            {
                _logger.LogWarning($"Unable to prejoin character '{character.Name}' for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                    "Reason: bad bank code.");
                client.LoginProtectValue = new Random().Next(0, 1000);
                _clusterPacketFactory.SendLoginProtect(client, client.LoginProtectValue);
                return;
            }

            _clusterPacketFactory.SendJoinWorld(client);
            _logger.LogInformation($"Character '{character.Name}' has prejoin successfully the game for user '{packet.Username}' from {client.Socket.RemoteEndPoint}.");
        }
    }
}
