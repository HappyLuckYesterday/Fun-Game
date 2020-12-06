using System.Collections.Generic;

namespace Rhisis.Network.Core
{
    /// <summary>
    /// Defines the server types.
    /// </summary>
    public enum ServerType
    {
        /// <summary>
        /// Unknown server type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Cluster Server.
        /// </summary>
        Cluster,

        /// <summary>
        /// World Server.
        /// </summary>
        World
    }

    public interface IServerInfo
    {
        /// <summary>
        /// Gets or sets the server's unique Id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets or sets the server's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets the server's host name or IP address.
        /// </summary>
        string Host { get; }

        /// <summary>
        /// Gets or sets the server's listening port.
        /// </summary>
        int Port { get; }
    }
}
