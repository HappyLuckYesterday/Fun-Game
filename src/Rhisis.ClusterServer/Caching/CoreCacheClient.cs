using LiteNetwork.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Configuration;
using Rhisis.Core.Configuration.Cluster;
using Rhisis.Core.Extensions;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Caching;

public sealed class CoreCacheClient : LiteClient
{
    private readonly ILogger<CoreCacheClient> _logger;
    private readonly IOptions<ClusterServerOptions> _clusterOptions;
    private readonly IOptions<CoreCacheClientOptions> _coreClientOptions;
    private readonly IServiceProvider _serviceProvider;

    private string Name => _clusterOptions.Value.Name;

    public CoreCacheClient(LiteClientOptions options,
        ILogger<CoreCacheClient> logger,
        IOptions<ClusterServerOptions> clusterOptions,
        IOptions<CoreCacheClientOptions> coreClientOptions,
        IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
        _logger = logger;
        _clusterOptions = clusterOptions;
        _coreClientOptions = coreClientOptions;
        _serviceProvider = serviceProvider;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        using BinaryReader reader = new(new MemoryStream(packetBuffer));

        CorePacketType packetType = (CorePacketType)reader.ReadByte();

        try
        {
            string message = reader.BaseStream.IsEndOfStream() ? null : reader.ReadString();

            switch (packetType)
            {
                case CorePacketType.Handshake:
                    OnHandshake();
                    break;
                case CorePacketType.AuthenticationResult:
                    OnAuthenticationResult(JsonSerializer.Deserialize<ServerAuthenticationResultPacket>(message));
                    break;
                default:
                    _logger.LogWarning($"No handler for packet: '{packetType}'.");
                    break;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error occured while parsing packet '{packetType}'.");
        }

        return Task.CompletedTask;
    }

    protected override void OnConnected()
    {
        _logger.LogInformation($"Cluster '{_clusterOptions.Value.Name}' connected to Core Cache server.");
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation($"Cluster '{_clusterOptions.Value.Name}' disconnected from Core Cache server.");
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

    private void OnHandshake()
    {
        ServerAuthenticationPacket packet = new(Name,
            _clusterOptions.Value.Ip,
            _clusterOptions.Value.Port,
            _coreClientOptions.Value.MasterPassword);

        Send(CorePacketType.Authenticate, packet);
    }

    private void OnAuthenticationResult(ServerAuthenticationResultPacket packet)
    {
        switch (packet.Result)
        {
            case CoreAuthenticationResult.Success:
                _logger.LogInformation($"Server '{Name}' authenticated to core cache server.");
                ICluster cluster = _serviceProvider.GetRequiredService<ICluster>();

                cluster.SendChannels();
                break;
            case CoreAuthenticationResult.ClusterExists:
                _logger.LogWarning($"A server with name '{Name}' is already connected to core cache server.");
                break;
            case CoreAuthenticationResult.WrongMasterPassword:
                _logger.LogWarning($"Authentication failed: wrong master password.");
                break;
        }
    }
}
