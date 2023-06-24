using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Core.Configuration.Cluster;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol.Networking;
using System.Xml.Linq;

namespace Rhisis.ClusterServer.Caching.Client.Handlers;

internal sealed class HandshakeHandler : IFFInterServerConnectionHandler<CoreCacheClient, HandshakePacket>
{
    private readonly IOptions<ClusterServerOptions> _clusterOptions;
    private readonly IOptions<CoreCacheClientOptions> _coreClientOptions;

    public HandshakeHandler(IOptions<ClusterServerOptions> clusterOptions, IOptions<CoreCacheClientOptions> coreClientOptions)
    {
        _clusterOptions = clusterOptions;
        _coreClientOptions = coreClientOptions;
    }

    public void Execute(CoreCacheClient user, HandshakePacket message)
    {
        ServerAuthenticationPacket packet = new(user.Name,
            _clusterOptions.Value.Ip,
            _clusterOptions.Value.Port,
            _coreClientOptions.Value.MasterPassword);

        user.SendMessage(packet);
    }
}
