using System;

namespace Rhisis.LoginServer.Abstractions
{
    /// <summary>
    /// Provides an interface for the Login Server instance.
    /// </summary>
    public interface ILoginServer
    {
        /// <summary>
        /// Gets a connected client by his username.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <returns></returns>
        ILoginUser GetClientByUsername(string username);

        /// <summary>
        /// Verify if a client is connected to the login server.
        /// </summary>
        /// <param name="username">Client username</param>
        /// <returns></returns>
        bool IsClientConnected(string username);

        /// <summary>
        /// Disconnects a user with the specified guid.
        /// </summary>
        /// <param name="userId">User id to disconnect.</param>
        void DisconnectUser(Guid userId);
    }
}