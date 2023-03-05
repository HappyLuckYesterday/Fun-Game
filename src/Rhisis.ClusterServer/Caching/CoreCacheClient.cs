using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Core;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Caching;

public sealed class CoreCacheClient : LiteClient
{
    private readonly ILogger<CoreCacheClient> _logger;
    private readonly IOptions<ClusterServerOptions> _clusterOptions;
    private readonly IWorldChannelCache _worldChannelCache;

    public CoreCacheClient(LiteClientOptions options, ILogger<CoreCacheClient> logger, IOptions<ClusterServerOptions> clusterOptions, IWorldChannelCache worldChannelCache, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
        _logger = logger;
        _clusterOptions = clusterOptions;
        _worldChannelCache = worldChannelCache;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        return Task.CompletedTask;
    }

    protected override void OnConnected()
    {
        _logger.LogInformation("Cluster cache client connected.");
        SendClusterInformation();
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation("Cluster cache client disconnected.");
    }

    public void Send(CorePacketType packet, object message = null)
    {
        using BinaryWriter writer = new(new MemoryStream());
        writer.Write((byte)packet);

        if (message is not null)
        {
            writer.Write(JsonSerializer.Serialize(message));
        }

        Send(writer.BaseStream);
    }

    public void SendClusterInformation()
    {
        ClusterInfo cluster = new()
        {
            Id = _clusterOptions.Value.Name.GetHashCode(),
            Name = _clusterOptions.Value.Name,
            Ip = _clusterOptions.Value.Ip,
            Port = _clusterOptions.Value.Port,
            IsEnabled = true,
            Channels = _worldChannelCache.GetWorldChannels()?.ToArray() ?? Array.Empty<WorldChannelInfo>()
        };

        Send(CorePacketType.UpdateClusterInfo, new ClusterInfoUpdatePacket(cluster));
    }
}