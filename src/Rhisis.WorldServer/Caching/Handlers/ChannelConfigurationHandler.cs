using Rhisis.Game.Protocol.Packets.Core;
using Rhisis.Protocol.Networking;
using System;
using Microsoft.Extensions.Logging;
using Rhisis.Game.Resources;
using Rhisis.Game;

namespace Rhisis.WorldServer.Caching.Handlers;

internal sealed class ChannelConfigurationHandler : IFFInterServerConnectionHandler<ClusterCacheClient, ChannelConfigurationPacket>
{
    private readonly ILogger<ChannelConfigurationHandler> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ChannelConfigurationHandler(ILogger<ChannelConfigurationHandler> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void Execute(ClusterCacheClient user, ChannelConfigurationPacket message)
    {
        _logger.LogInformation($"Receiving cluster game options.");

        GameOptions.Current.Rates = message.Rates;
        GameOptions.Current.Messenger = message.MessengerOptions;
        GameOptions.Current.Customization = message.CustomizationOptions;
        GameOptions.Current.Drops = message.DropOptions;
        GameOptions.Current.DefaultCharacter = message.DefaultCharacterOptions;
        GameResources.Current.Maps.Load(message.Maps);
        MapManager.Current.Initialize(_serviceProvider);
    }
}