using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;

namespace Rhisis.ClusterServer.Core.Handlers
{
    [Handler]
    public class AuthenticationResultHandler
    {
        private readonly ILogger<AuthenticationResultHandler> _logger;

        public AuthenticationResultHandler(ILogger<AuthenticationResultHandler> logger)
        {
            _logger = logger;
        }

        [HandlerAction(CorePacketType.AuthenticationResult)]
        public void OnExecute(ClusterCoreClient _, INetPacketStream packet)
        {
            var result = (CoreAuthenticationResultType)packet.ReadByte();

            switch (result)
            {
                case CoreAuthenticationResultType.Success:
                    {
                        _logger.LogInformation("Cluster Core client authenticated successfully.");
                        return;
                    }
                case CoreAuthenticationResultType.FailedClusterExists:
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: an other Cluster server (with the same id) is already connected.");
                    break;
                case CoreAuthenticationResultType.FailedUnknownServer:
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                    break;
                default:
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: Cannot recognize Core server. You probably have to update all servers.");
                    break;
            }

            Environment.Exit((int)result);
        }
    }
}
