using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol;
using Rhisis.Protocol.Core;
using Rhisis.Protocol.Packets.Client.Login;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.LoginServer.Handlers
{
    [Handler]
    public class CloseExistingConnectionHandler
    {
        private readonly ILoginServer _loginServer;
        private readonly ICoreServer _coreServer;

        public CloseExistingConnectionHandler(ILoginServer loginServer, ICoreServer coreServer)
        {
            _loginServer = loginServer;
            _coreServer = coreServer;
        }

        /// <summary>
        /// Closes an existing connection.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="closeConnectionPacket">Close connection packet.</param>
        [HandlerAction(PacketType.CLOSE_EXISTING_CONNECTION)]
        public void Execute(ILoginUser _, CloseConnectionPacket closeConnectionPacket)
        {
            var otherConnectedClient = _loginServer.GetClientByUsername(closeConnectionPacket.Username);

            if (otherConnectedClient is null)
            {
                throw new InvalidOperationException($"Cannot find user with username '{closeConnectionPacket.Username}'.");
            }

            using var packet = new CorePacket();
            packet.WriteByte((byte)CorePacketType.DisconnectUserFromCluster);
            packet.WriteInt32(otherConnectedClient.UserId);

            _coreServer.SendToClusters(packet);
        }
    }
}