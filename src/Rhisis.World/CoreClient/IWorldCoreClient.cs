﻿using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Network.Core;

namespace Rhisis.World.CoreClient
{
    public interface IWorldCoreClient : ICoreClient
    {
        /// <summary>
        /// Gets the world server configuration.
        /// </summary>
        WorldConfiguration WorldServerConfiguration { get; }

        /// <summary>
        /// Gets the core client configuration.
        /// </summary>
        CoreConfiguration CoreClientConfiguration { get; }

        /// <summary>
        /// Gets the remote end point (IP and port) for this client.
        /// </summary>
        string RemoteEndPoint { get; }
    }
}
