using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Configuration;
using Rhisis.Core.Extensions;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Caching;

public class WorldChannelUser : FFInterServerConnection
{
    private readonly ILogger<WorldChannelUser> _logger;
    private readonly IOptions<ClusterCacheServerOptions> _clusterCacheServerOptions;
    private readonly ICluster _cluster;

    public string Name { get; private set; }

    public bool IsAuthenticated { get; private set; }

    public WorldChannelUser(ILogger<WorldChannelUser> logger, IOptions<ClusterCacheServerOptions> clusterCacheServerOptions, ICluster cluster)
    {
        _logger = logger;
        _clusterCacheServerOptions = clusterCacheServerOptions;
        _cluster = cluster;
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
                case CorePacketType.Authenticate:
                    OnAuthenticate(JsonSerializer.Deserialize<WorldChannelAuthenticationPacket>(message));
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

        return base.HandleMessageAsync(packetBuffer);
    }

    protected override void OnConnected()
    {
        _logger.LogInformation($"New world channel connected to cluster server ({Id}).");

        Send(CorePacketType.Handshake);
    }

    protected override void OnDisconnected()
    {
        _logger.LogInformation($"World channel '{Name}' disconnected.");

        if (IsAuthenticated)
        {
            _cluster.RemoveChannel(Name);

            IsAuthenticated = false;
        }
    }

    private void OnAuthenticate(WorldChannelAuthenticationPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet, nameof(packet));

        if (IsAuthenticated)
        {
            throw new InvalidOperationException("Cluster is already authenticated.");
        }

        if (!_clusterCacheServerOptions.Value.MasterPassword.Equals(packet.MasterPassword))
        {
            _logger.LogWarning($"Cluster '{packet.Name}' tried to authenticated with wrong password.");

            Send(CorePacketType.AuthenticationResult, new ServerAuthenticationResultPacket(CoreAuthenticationResult.WrongMasterPassword));
            Disconnect();
        }

        if (string.IsNullOrEmpty(Name) && _cluster.HasChannel(packet.Name))
        {
            _logger.LogWarning($"A cluster with name '{packet.Name}' is already connected.");

            Send(CorePacketType.AuthenticationResult, new ServerAuthenticationResultPacket(CoreAuthenticationResult.ClusterExists));
            Dispose();
        }

        WorldChannel channel = new(connectionId: Id)
        {
            Id = packet.Name.GetHashCode(),
            Name = packet.Name,
            Ip = packet.Ip,
            Port = packet.Port,
            IsEnabled = true,
            MaximumUsers = packet.MaximumUsers
        };

        _cluster.AddChannel(channel);

        Send(CorePacketType.AuthenticationResult, new ServerAuthenticationResultPacket(CoreAuthenticationResult.Success));

        Name = channel.Name;
        IsAuthenticated = true;

        _logger.LogInformation($"World channel '{Name}' authenticated to cluster server.");
    }
}
