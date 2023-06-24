using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol;
using Rhisis.Protocol.Networking;
using System;
using System.Collections.Generic;

namespace Rhisis.LoginServer.Core.Handlers;

internal class ServerAuthenticationHandler : IFFInterServerConnectionHandler<CoreCacheUser, ServerAuthenticationPacket>
{
    private readonly ILogger<ServerAuthenticationHandler> _logger;
    private readonly IOptions<CoreCacheServerOptions> _coreServerOptions;
    private readonly IClusterCache _clusterCache;

    public ServerAuthenticationHandler(ILogger<ServerAuthenticationHandler> logger, IOptions<CoreCacheServerOptions> coreServerOptions, IClusterCache clusterCache)
    {
        _logger = logger;
        _coreServerOptions = coreServerOptions;
        _clusterCache = clusterCache;
    }

    public void Execute(CoreCacheUser user, ServerAuthenticationPacket message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (user.IsAuthenticated)
        {
            throw new InvalidOperationException("Cluster is already authenticated.");
        }

        if (!_coreServerOptions.Value.MasterPassword.Equals(message.MasterPassword))
        {
            _logger.LogWarning($"Cluster '{message.Name}' tried to authenticated with wrong password.");

            user.SendMessage(new ServerAuthenticationResultPacket(CoreAuthenticationResult.WrongMasterPassword));
            user.Disconnect();
        }

        if (user.Cluster is null && _clusterCache.HasCluster(message.Name))
        {
            _logger.LogWarning($"A cluster with name '{message.Name}' is already connected.");

            user.SendMessage(new ServerAuthenticationResultPacket(CoreAuthenticationResult.ClusterExists));
            user.Dispose();
        }

        user.Cluster = new()
        {
            Name = message.Name,
            Ip = message.Ip,
            Port = message.Port,
            IsEnabled = true,
            Channels = new List<WorldChannelInfo>()
        };

        _clusterCache.AddCluster(user.Cluster);

        user.SendMessage(new ServerAuthenticationResultPacket(CoreAuthenticationResult.Success));
    }
}
