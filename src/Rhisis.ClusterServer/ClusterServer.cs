using LiteNetwork.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.ClusterServer.Caching;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Rhisis.ClusterServer;

public sealed class ClusterServer : LiteServer<ClusterUser>, ICluster
{
    private readonly ILogger<ClusterServer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, WorldChannel> _channels = new();

    private readonly WorldChannelCacheServer _worldChannelCacheServer;
    private readonly CoreCacheClient _coreCacheClient;

    public string Name => throw new NotImplementedException();

    public IReadOnlyList<WorldChannel> Channels => _channels.Values.ToImmutableList();

    public ClusterServer(LiteServerOptions options, ILogger<ClusterServer> logger, WorldChannelCacheServer worldChannelCacheServer, CoreCacheClient coreCacheClient, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
        _logger = logger;
        _worldChannelCacheServer = worldChannelCacheServer;
        _coreCacheClient = coreCacheClient;
        _serviceProvider = serviceProvider;
    }

    protected override void OnBeforeStart()
    {
        if (_serviceProvider is not null)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IAccountDatabase accountDatabase = scope.ServiceProvider.GetService<IAccountDatabase>();
            IGameDatabase gameDatabase = scope.ServiceProvider.GetService<IGameDatabase>();

            accountDatabase.Migrate();
            gameDatabase.Migrate();
        }

        base.OnBeforeStart();
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"Login Server listening on port {Options.Port}.");
    }

    protected override void OnError(ClusterUser connection, Exception exception)
    {
        _logger.LogError(exception, $"An exception occured in {typeof(ClusterServer).Name}.");
    }

    public void AddChannel(WorldChannel channel)
    {
        ArgumentNullException.ThrowIfNull(channel, nameof(channel));

        if (!_channels.TryAdd(channel.Name, channel))
        {
            throw new InvalidOperationException($"Channel '{channel.Name}' already exists.");
        }

        _coreCacheClient.Send(CorePacketType.AddChannel, new ClusterAddChannelPacket(new WorldChannelInfo
        {
            Id = channel.Id,
            Ip = channel.Ip,
            Port = channel.Port,
            Name = channel.Name,
            MaximumUsers = channel.MaximumUsers,
            ConnectedUsers = channel.ConnectedUsers,
            IsEnabled = channel.IsEnabled,
        }));
    }

    public void RemoveChannel(string channelName)
    {
        ArgumentException.ThrowIfNullOrEmpty(channelName, nameof(channelName));

        if (!_channels.TryRemove(channelName, out WorldChannel channel))
        {
            throw new InvalidOperationException($"Failed to remove channel with name '{channelName}'.");
        }

        _worldChannelCacheServer.DisconnectUser(channel.ConnectionId);
        _coreCacheClient.Send(CorePacketType.RemoveChannel, new ClusterRemoveChannelPacket(channel.Name));
    }

    public WorldChannel GetChannel(string channelName) => _channels.GetValueOrDefault(channelName);

    public bool HasChannel(string channelName) => GetChannel(channelName) is not null;

    public void SendChannels()
    {
        foreach (WorldChannel channel in _channels.Values)
        {
            _coreCacheClient.Send(CorePacketType.AddChannel, new ClusterAddChannelPacket(new WorldChannelInfo
            {
                Id = channel.Id,
                Ip = channel.Ip,
                Port = channel.Port,
                Name = channel.Name,
                MaximumUsers = channel.MaximumUsers,
                ConnectedUsers = channel.ConnectedUsers,
                IsEnabled = channel.IsEnabled,
            }));
        }
    }
}
