using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol;
using Rhisis.Protocol.Core.Servers;
using System.Collections.Generic;

namespace Rhisis.LoginServer.Packets
{
    public interface ILoginPacketFactory
    {
        /// <summary>
        /// Send a welcome packet to the client.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="sessionId">Session id</param>
        void SendWelcome(ILoginUser client, uint sessionId);

        /// <summary>
        /// Send a pong response to the client.
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="time">Current time</param>
        void SendPong(ILoginUser client, int time);

        /// <summary>
        /// Sends a login error.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="error"></param>
        void SendLoginError(ILoginUser client, ErrorType error);

        /// <summary>
        /// Sends the available server list.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="clusters"></param>
        void SendServerList(ILoginUser client, string username, IEnumerable<Cluster> clusters);
    }
}
