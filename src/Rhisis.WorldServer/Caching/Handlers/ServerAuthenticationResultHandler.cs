using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol.Networking;
using System;

namespace Rhisis.WorldServer.Caching.Handlers;

internal sealed class ServerAuthenticationResultHandler : IFFInterServerConnectionHandler<ClusterCacheClient, ServerAuthenticationResultPacket>
{
    private readonly ILogger<ServerAuthenticationResultHandler> _logger;
    private readonly IOptions<WorldChannelServerOptions> _channelOptions;

    public ServerAuthenticationResultHandler(ILogger<ServerAuthenticationResultHandler> logger, IOptions<WorldChannelServerOptions> channelOptions)
    {
        _logger = logger;
        _channelOptions = channelOptions;
    }

    public void Execute(ClusterCacheClient user, ServerAuthenticationResultPacket message)
    {
        switch (message.Result)
        {
            case CoreAuthenticationResult.Success:
                _logger.LogInformation($"Server '{_channelOptions.Value.Name}' authenticated to cluster cache server.");
                break;
            case CoreAuthenticationResult.WorldChannelExists:
                _logger.LogWarning($"A server with name '{_channelOptions.Value.Name}' is already connected to cluster cache server.");
                break;
            case CoreAuthenticationResult.WrongMasterPassword:
                _logger.LogWarning($"Authentication failed: wrong master password.");
                break;
            default: throw new NotImplementedException();
        }
    }
}
