using Rhisis.LoginServer.Client;
using Rhisis.Network.Core.Servers;
using Sylver.Network.Server;
using System.Collections.Generic;

namespace Rhisis.LoginServer
{
    /// <summary>
    /// Provides an interface for the Login Server instance.
    /// </summary>
    public interface ILoginServer : INetServer
    {
        /// <summary>
        /// Gets the connected clusters list.
        /// </summary>
        IReadOnlyDictionary<int, Cluster> ConnectedClusters { get; }

        /// <summary>
        /// Gets a connected client by his username.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <returns></returns>
        ILoginClient GetClientByUsername(string username);

        /// <summary>
        /// Verify if a client is connected to the login server.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <returns></returns>
        bool IsClientConnected(string username);
    }
}