using Microsoft.Extensions.Logging;
using Rhisis.Protocol.Core;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.ClusterCache.Handlers;

[Handler]
internal class AuthenticationResultHandler
{
    private readonly ILogger<AuthenticationResultHandler> _logger;

    public AuthenticationResultHandler(ILogger<AuthenticationResultHandler> logger)
    {
        _logger = logger;
    }

    [HandlerAction(CorePacketType.AuthenticationResult)]
    public void OnExecute(IClusterCacheClient client, CorePacket packet)
    {
        var result = (CoreAuthenticationResultType)packet.ReadByte();

        switch (result)
        {
            case CoreAuthenticationResultType.Success:
                {
                    _logger.LogInformation("World Core client authenticated successfully.");
                    return;
                }
            case CoreAuthenticationResultType.FailedWorldExists:
                _logger.LogCritical("Unable to authenticate World Core client. Reason: an other World server (with the same id) is already connected.");
                break;
            case CoreAuthenticationResultType.FailedUnknownServer:
                _logger.LogCritical("Unable to authenticate World Core client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                break;
            default:
                _logger.LogCritical("Unable to authenticate World Core client. Reason: Cannot recognize Core server. You probably have to update all servers.");
                break;
        }

        Environment.Exit((int)result);
    }
}
