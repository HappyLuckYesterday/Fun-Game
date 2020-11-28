using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Client;
using Rhisis.ClusterServer.Packets;
using Rhisis.ClusterServer.Structures;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.Cluster;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers
{
    [Handler]
    public class DeletePlayerHandler : ClusterHandlerBase
    {
        private readonly ILogger<DeletePlayerHandler> _logger;
        private readonly IClusterPacketFactory _clusterPacketFactory;

        public DeletePlayerHandler(ILogger<DeletePlayerHandler> logger, IRhisisDatabase database, IClusterPacketFactory clusterPacketFactory)
            : base(database)
        {
            _logger = logger;
            _clusterPacketFactory = clusterPacketFactory;
        }

        [HandlerAction(PacketType.DEL_PLAYER)]
        public void Execute(IClusterClient client, DeletePlayerPacket packet)
        {
            DbUser dbUser = Database.Users.FirstOrDefault(x => x.Username == packet.Username && x.Password == packet.Password);

            if (dbUser is null)
            {
                _logger.LogWarning($"[SECURITY] Unable to create new character for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                    "Reason: bad presented credentials compared to the database.");
                client.Disconnect();
                return;
            }

            if (!string.Equals(packet.Password, packet.PasswordConfirmation, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Unable to delete character id '{packet.CharacterId}' for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                    "Reason: passwords entered do not match.");
                _clusterPacketFactory.SendClusterError(client, ErrorType.WRONG_PASSWORD);
                return;
            }

            DbCharacter characterToDelete = Database.Characters.FirstOrDefault(x => x.Id == packet.CharacterId);

            // Check if character exist.
            if (characterToDelete is null)
            {
                _logger.LogWarning($"[SECURITY] Unable to delete character id '{packet.CharacterId}' for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                    "Reason: user doesn't have any character with this id.");
                client.Disconnect();
                return;
            }

            if (characterToDelete.IsDeleted)
            {
                _logger.LogWarning($"[SECURITY] Unable to delete character id '{packet.CharacterId}' for user '{packet.Username}' from {client.Socket.RemoteEndPoint}. " +
                       "Reason: character is already deleted.");
                return;
            }

            characterToDelete.IsDeleted = true;

            Database.Characters.Update(characterToDelete);
            Database.SaveChanges();

            _logger.LogInformation($"Character '{characterToDelete.Name}' has been deleted successfully for user '{packet.Username}' from {client.Socket.RemoteEndPoint}.");

            IEnumerable<ClusterCharacter> dbCharacters = GetCharacters(dbUser.Id);

            _clusterPacketFactory.SendPlayerList(client, packet.AuthenticationKey, dbCharacters);
        }
    }
}
