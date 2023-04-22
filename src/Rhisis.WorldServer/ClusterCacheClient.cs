using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Core.Extensions;
using Rhisis.Game;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Game.Resources;
using Rhisis.Protocol;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.WorldServer;

public class ClusterCacheClient : LiteClient
{
    private readonly ILogger<ClusterCacheClient> _logger;
    private readonly IOptions<WorldChannelServerOptions> _channelOptions;
    private readonly IServiceProvider _serviceProvider;

    public ClusterCacheClient(LiteClientOptions options, ILogger<ClusterCacheClient> logger, IOptions<WorldChannelServerOptions> channelOptions, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
        _logger = logger;
        _channelOptions = channelOptions;
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
                case CorePacketType.ChannelConfiguration:
                    OnChannelConfiguration(JsonSerializer.Deserialize<ChannelConfigurationPacket>(message));
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
        _logger.LogInformation($"Channel '{_channelOptions.Value.Name}' connected to cluster cache server.");
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation($"Channel '{_channelOptions.Value.Name}' disconnected from cluster cache server.");
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
        WorldChannelAuthenticationPacket packet = new(
            _channelOptions.Value.Cluster.Name,
            _channelOptions.Value.Name,
            _channelOptions.Value.Ip,
            _channelOptions.Value.Port,
            _channelOptions.Value.Cluster.MasterPassword,
            _channelOptions.Value.MaximumUsers);

        Send(CorePacketType.Authenticate, packet);
    }

    private void OnAuthenticationResult(ServerAuthenticationResultPacket packet)
    {
        switch (packet.Result)
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

    private void OnChannelConfiguration(ChannelConfigurationPacket packet)
    {
        _logger.LogInformation($"Receiving cluster game options.");

        GameOptions.Current.Rates = packet.Rates;
        GameOptions.Current.Messenger = packet.MessengerOptions;
        GameOptions.Current.Customization = packet.CustomizationOptions;
        GameOptions.Current.Drops = packet.DropOptions;
        GameResources.Current.Maps.Load(packet.Maps);
        MapManager.Current.Initialize(_serviceProvider);
    }
}
