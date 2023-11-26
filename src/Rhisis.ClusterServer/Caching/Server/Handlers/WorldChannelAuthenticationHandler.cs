using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Configuration;
using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol.Networking;
using System;
using System.Linq;

namespace Rhisis.ClusterServer.Caching.Server.Handlers;

internal sealed class WorldChannelAuthenticationHandler : IFFInterServerConnectionHandler<ClusterCacheUser, WorldChannelAuthenticationPacket>
{
    private readonly ILogger<WorldChannelAuthenticationHandler> _logger;
    private readonly IOptions<ClusterCacheServerOptions> _clusterCacheServerOptions;
    private readonly ICluster _cluster;

    public WorldChannelAuthenticationHandler(ILogger<WorldChannelAuthenticationHandler> logger, IOptions<ClusterCacheServerOptions> clusterCacheServerOptions, ICluster cluster)
    {
        _logger = logger;
        _clusterCacheServerOptions = clusterCacheServerOptions;
        _cluster = cluster;
    }

    public void Execute(ClusterCacheUser user, WorldChannelAuthenticationPacket message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        if (user.IsAuthenticated)
        {
            throw new InvalidOperationException("Cluster is already authenticated.");
        }

        if (!_clusterCacheServerOptions.Value.MasterPassword.Equals(message.MasterPassword))
        {
            _logger.LogWarning($"Cluster '{message.Name}' tried to authenticated with wrong password.");

            user.SendMessage(new ServerAuthenticationResultPacket(CoreAuthenticationResult.WrongMasterPassword));
            user.Disconnect();
        }

        if (string.IsNullOrEmpty(user.Name) && _cluster.HasChannel(message.Name))
        {
            _logger.LogWarning($"A cluster with name '{message.Name}' is already connected.");

            user.SendMessage(new ServerAuthenticationResultPacket(CoreAuthenticationResult.ClusterExists));
            user.Dispose();
        }

        WorldChannel channel = new(connectionId: user.Id)
        {
            Id = message.Name.GetHashCode(),
            Name = message.Name,
            Ip = message.Ip,
            Port = message.Port,
            IsEnabled = true,
            MaximumUsers = message.MaximumUsers
        };

        _cluster.AddChannel(channel);

        user.SendMessage(new ServerAuthenticationResultPacket(CoreAuthenticationResult.Success));
        user.SendMessage(new ChannelConfigurationPacket(
            _cluster.Configuration.Rates,
            _cluster.Configuration.Messenger,
            _cluster.Configuration.Customization,
            _cluster.Configuration.Drops,
            _cluster.Configuration.DefaultCharacter,
        _cluster.Configuration.Maps.ToArray()));

        user.Name = channel.Name;
        user.IsAuthenticated = true;

        _logger.LogInformation($"World channel '{user.Name}' authenticated to cluster server.");
    }
}