using Rhisis.LoginServer.Client;
using Rhisis.Network;
using Rhisis.Network.Packets.Login;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.LoginServer.Handlers
{
    [Handler]
    public class CloseExistingConnectionHandler
    {
        private readonly ILoginServer _loginServer;

        public CloseExistingConnectionHandler(ILoginServer loginServer)
        {
            _loginServer = loginServer;
        }

        /// <summary>
        /// Closes an existing connection.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="closeConnectionPacket">Close connection packet.</param>
        [HandlerAction(PacketType.CLOSE_EXISTING_CONNECTION)]
        public void Execute(ILoginClient _, CloseConnectionPacket closeConnectionPacket)
        {
            var otherConnectedClient = _loginServer.GetClientByUsername(closeConnectionPacket.Username);

            if (otherConnectedClient is null)
            {
                throw new InvalidOperationException($"Cannot find user with username '{closeConnectionPacket.Username}'.");
            }

            // TODO: disconnect client from server and ISC.
        }
    }
}