using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Abstractions.Server;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.LoginServer.Core.Handlers;

[Handler]
public class AuthenticationRequestHandler
{
    private readonly IOptions<CoreOptions> _coreOptions;
    private readonly ILogger<AuthenticationRequestHandler> _logger;

    public AuthenticationRequestHandler(IOptions<CoreOptions> coreOptions, ILogger<AuthenticationRequestHandler> logger)
    {
        _coreOptions = coreOptions;
        _logger = logger;
    }

    [HandlerAction(CorePacketType.AuthenticationRequest)]
    public void OnExecute(CoreUser coreClient, CorePacket packet)
    {
        var corePassword = packet.ReadString();

        if (!_coreOptions.Value.Password.ToLowerInvariant().Equals(corePassword.ToLowerInvariant()))
        {
            _logger.LogError("Failed to authenticate cluster: wrong password.");
            SendAuthenticationResult(coreClient, CoreAuthenticationResultType.FailedWrongPassword);
            return;
        }

        coreClient.Cluster.Id = packet.ReadInt32();
        coreClient.Cluster.Name = packet.ReadString();
        coreClient.Cluster.Host = packet.ReadString();
        coreClient.Cluster.Port = packet.ReadUInt16();
        var channelsCount = packet.ReadByte();

        for (int i = 0; i < channelsCount; i++)
        {
            var channel = new WorldChannel()
            {
                Id = packet.ReadInt32(),
                Name = packet.ReadString(),
                Host = packet.ReadString(),
                Port = packet.ReadUInt16(),
                MaximumUsers = packet.ReadInt32(),
                ConnectedUsers = packet.ReadInt32()
            };

            if (coreClient.Cluster.Channels.Any(x => x.Name == channel.Name))
            {
                _logger.LogError($"Failed to authenticate cluster '{channel.Name}': Cluster with same name already exists.");
                SendAuthenticationResult(coreClient, CoreAuthenticationResultType.FailedClusterExists);
                continue;
            }

            coreClient.Cluster.Channels.Add(channel);
        }

        if (coreClient.Cluster is not null)
        {
            _logger.LogInformation($"Cluster '{coreClient.Cluster.Name}' successfuly connected to core server.");
            SendAuthenticationResult(coreClient, CoreAuthenticationResultType.Success);
        }
    }

    private static void SendAuthenticationResult(CoreUser client, CoreAuthenticationResultType result)
    {
        using var packet = new CorePacket();

        packet.WriteByte((byte)CorePacketType.AuthenticationResult);
        packet.WriteByte((byte)result);

        client.Send(packet);
    }
}
