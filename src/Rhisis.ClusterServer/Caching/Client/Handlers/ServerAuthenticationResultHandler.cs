using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol.Networking;
using System;

namespace Rhisis.ClusterServer.Caching.Client.Handlers;

internal sealed class ServerAuthenticationResultHandler : IFFInterServerConnectionHandler<CoreCacheClient, ServerAuthenticationResultPacket>
{
    private readonly ILogger<ServerAuthenticationResultHandler> _logger;
    private readonly ICluster _cluster;

    public ServerAuthenticationResultHandler(ILogger<ServerAuthenticationResultHandler> logger, ICluster cluster)
    {
        _logger = logger;
        _cluster = cluster;
    }

    public void Execute(CoreCacheClient user, ServerAuthenticationResultPacket message)
    {
        switch (message.Result)
        {
            case CoreAuthenticationResult.Success:
                _logger.LogInformation($"Server '{user.Name}' authenticated to core cache server.");
                _cluster.SendChannels();
                break;
            case CoreAuthenticationResult.ClusterExists:
                _logger.LogWarning($"A server with name '{user.Name}' is already connected to core cache server.");
                break;
            case CoreAuthenticationResult.WrongMasterPassword:
                _logger.LogWarning($"Authentication failed: wrong master password.");
                break;
            default: throw new NotImplementedException();
        }
    }
}
