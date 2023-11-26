using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol.Networking;

namespace Rhisis.WorldServer.Caching.Handlers;

internal sealed class HandshakeHandler : IFFInterServerConnectionHandler<ClusterCacheClient, HandshakePacket>
{
    private readonly IOptions<WorldChannelServerOptions> _channelOptions;

    public HandshakeHandler(IOptions<WorldChannelServerOptions> channelOptions)
    {
        _channelOptions = channelOptions;
    }

    public void Execute(ClusterCacheClient user, HandshakePacket message)
    {
        WorldChannelAuthenticationPacket packet = new(
            _channelOptions.Value.Cluster.Name,
            _channelOptions.Value.Name,
            _channelOptions.Value.Ip,
            _channelOptions.Value.Port,
            _channelOptions.Value.Cluster.MasterPassword,
            _channelOptions.Value.MaximumUsers);

        user.SendMessage(packet);
    }
}
