using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.ClusterServer.Core;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.ClusterServer.Cache.Handlers;

[Handler]
public class AuthenticationRequestHandler
{
    private readonly ILogger<AuthenticationRequestHandler> _logger;
    private readonly IOptions<ClusterOptions> _clusterOptions;
    private readonly IClusterCacheServer _clusterCacheServer;
    private readonly ICoreClient _coreClient;

    public AuthenticationRequestHandler(ILogger<AuthenticationRequestHandler> logger, IOptions<ClusterOptions> clusterOptions, 
        IClusterCacheServer clusterCacheServer,
        ICoreClient coreClient)
    {
        _logger = logger;
        _clusterOptions = clusterOptions;
        _clusterCacheServer = clusterCacheServer;
        _coreClient = coreClient;
    }

    [HandlerAction(CorePacketType.AuthenticationRequest)]
    public void OnExecute(ClusterCacheUser user, CorePacket packet)
    {
        string cachePassword = packet.ReadString();

        if (!_clusterOptions.Value.Cache.Password.ToLowerInvariant().Equals(cachePassword.ToLowerInvariant()))
        {
            _logger.LogError("Failed to authenticate world channel: wrong password.");
            SendAuthenticationResult(user, CoreAuthenticationResultType.FailedWrongPassword);
            return;
        }

        byte id = packet.ReadByte();
        string name = packet.ReadString();

        if (_clusterCacheServer.WorldChannels.Any(x => x.Id == id))
        {
            _logger.LogError($"Failed to authenticate world channel '{user.Channel.Name}': Already registered.");
            SendAuthenticationResult(user, CoreAuthenticationResultType.FailedWorldExists);
            return;
        }

        user.Channel.Id = id;
        user.Channel.Name = name;
        user.Channel.Host = packet.ReadString();
        user.Channel.Port = packet.ReadUInt16();
        user.Channel.ConnectedUsers = packet.ReadInt32();
        user.Channel.MaximumUsers = packet.ReadInt32();

        if (!string.IsNullOrEmpty(user.Channel.Name))
        {
            _logger.LogInformation($"World channel '{user.Channel.Name}' connected successfuly.");
            SendAuthenticationResult(user, CoreAuthenticationResultType.Success);
            _coreClient.UpdateWorldChannel(user.Channel);
        }
    }

    private static void SendAuthenticationResult(ClusterCacheUser client, CoreAuthenticationResultType result)
    {
        using var packet = new CorePacket();

        packet.WriteByte((byte)CorePacketType.AuthenticationResult);
        packet.WriteByte((byte)result);

        client.Send(packet);
    }
}
