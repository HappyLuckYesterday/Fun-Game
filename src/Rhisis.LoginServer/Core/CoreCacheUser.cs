using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Configuration;
using Rhisis.Core.Extensions;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.LoginServer.Core;

public sealed class CoreCacheUser : FFInterServerConnection
{
    private readonly ILogger<CoreCacheUser> _logger;
    private readonly IOptions<CoreCacheServerOptions> _coreServerOptions;
    private readonly IClusterCache _clusterCache;

    public bool IsAuthenticated => Cluster is not null;

    public ClusterInfo Cluster { get; private set; }

    public CoreCacheUser(ILogger<CoreCacheUser> logger, IOptions<CoreCacheServerOptions> coreServerOptions, IClusterCache clusterCache)
    {
        _logger = logger;
        _coreServerOptions = coreServerOptions;
        _clusterCache = clusterCache;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        using BinaryReader reader = new(new MemoryStream(packetBuffer));

        CorePacketType packetType = (CorePacketType)reader.ReadByte();

        try
        {
            _logger.LogTrace($"Received core packet '{packetType}'.");
            string message = reader.BaseStream.IsEndOfStream() ? null : reader.ReadString();

            switch (packetType)
            {
                case CorePacketType.Authenticate:
                    OnAuthenticate(JsonSerializer.Deserialize<ServerAuthenticationPacket>(message));
                    break;
                case CorePacketType.UpdateClusterInfo:
                    OnUpdateCluster();
                    break;
                case CorePacketType.AddChannel:
                    OnAddChannel(JsonSerializer.Deserialize<ClusterAddChannelPacket>(message));
                    break;
                case CorePacketType.UpdateChannel:
                    OnUpdateChannel(JsonSerializer.Deserialize<ClusterUpdateChannelPacket>(message));
                    break;
                case CorePacketType.RemoveChannel:
                    OnRemoveChannel(JsonSerializer.Deserialize<ClusterRemoveChannelPacket>(message));
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
        _logger.LogInformation($"New cluster connected to core server ({Id}).");

        Send(CorePacketType.Handshake);
    }

    private void OnAuthenticate(ServerAuthenticationPacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet, nameof(packet));

        if (IsAuthenticated)
        {
            throw new InvalidOperationException("Cluster is already authenticated.");
        }

        if (!_coreServerOptions.Value.MasterPassword.Equals(packet.MasterPassword))
        {
            _logger.LogWarning($"Cluster '{packet.Name}' tried to authenticated with wrong password.");

            Send(CorePacketType.AuthenticationResult, new ServerAuthenticationResultPacket(CoreAuthenticationResult.WrongMasterPassword));
            Disconnect();
        }

        if (Cluster is null && _clusterCache.HasCluster(packet.Name))
        {
            _logger.LogWarning($"A cluster with name '{packet.Name}' is already connected.");

            Send(CorePacketType.AuthenticationResult, new ServerAuthenticationResultPacket(CoreAuthenticationResult.ClusterExists));
            Dispose();
        }

        Cluster = new()
        {
            Name = packet.Name,
            Ip = packet.Ip,
            Port = packet.Port,
            IsEnabled = true,
            Channels = new List<WorldChannelInfo>()
        };

        _clusterCache.AddCluster(Cluster);

        Send(CorePacketType.AuthenticationResult, new ServerAuthenticationResultPacket(CoreAuthenticationResult.Success));
    }

    private void OnUpdateCluster()
    {
        IsAuthenticatedGuard();
    }

    private void OnAddChannel(ClusterAddChannelPacket packet)
    {
        IsAuthenticatedGuard();

        if (Cluster.Channels.Any(x => x.Name == packet.Channel.Name))
        {
            throw new InvalidOperationException($"Failed to add channel '{packet.Channel.Name}' to cluster '{Cluster.Name}' because a channel with the same name already exists.");
        }

        Cluster.Channels.Add(new WorldChannelInfo
        {
            Id = packet.Channel.Id,
            Ip = packet.Channel.Ip,
            Port = packet.Channel.Port,
            Name = packet.Channel.Name,
            IsEnabled = packet.Channel.IsEnabled,
            MaximumUsers = packet.Channel.MaximumUsers,
            ConnectedUsers = packet.Channel.ConnectedUsers
        });

        _logger.LogInformation($"World channel '{packet.Channel.Name}' added to '{Cluster.Name}'.");
    }

    private void OnUpdateChannel(ClusterUpdateChannelPacket packet)
    {
        IsAuthenticatedGuard();

        WorldChannelInfo channel = Cluster.Channels.SingleOrDefault(x => x.Name == packet.Channel.Name);

        if (channel is null)
        {
            throw new InvalidOperationException($"Channel with name '{packet.Channel.Name}' not found in cluster '{Cluster.Name}'.");
        }

        channel.ConnectedUsers = packet.Channel.ConnectedUsers;

        _logger.LogInformation($"World channel '{packet.Channel.Name}' updated for '{Cluster.Name}'.");
    }

    private void OnRemoveChannel(ClusterRemoveChannelPacket packet)
    {
        IsAuthenticatedGuard();

        _clusterCache.RemoveCluster(packet.ChannelName);

        _logger.LogInformation($"World channel '{packet.ChannelName}' removed from '{Cluster.Name}'.");
    }

    private void IsAuthenticatedGuard()
    {
        if (!IsAuthenticated)
        {
            throw new InvalidOperationException("Cluster must be authenticated to perform this operation.");
        }
    }
}