using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Protocol.Core;
using Rhisis.Protocol.Messages;
using Rhisis.Protocol.Packets.Cluster;
using Rhisis.WorldServer.Abstractions;
using Sylver.HandlerInvoker;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.WorldServer.ClusterCache;

internal class ClusterCacheClient : LiteClient, IClusterCacheClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new CoreMessageJsonConverter() },
        WriteIndented = true
    };

    private readonly ILogger<ClusterCacheClient> _logger;
    private readonly IOptions<WorldOptions> _worldOptions;
    private readonly IHandlerInvoker _handlerInvoker;
    private readonly IWorldServer _worldServer;

    public ClusterCacheClient(LiteClientOptions options, 
        IServiceProvider serviceProvider, 
        ILogger<ClusterCacheClient> logger, 
        IOptions<WorldOptions> worldOptions, 
        IHandlerInvoker handlerInvoker,
        IWorldServer worldServer)
        : base(options, serviceProvider)
    {
        _logger = logger;
        _worldOptions = worldOptions;
        _handlerInvoker = handlerInvoker;
        _worldServer = worldServer;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        try
        {
            using var packet = new CorePacket(packetBuffer);
            var packetHeader = (CorePacketType)packet.ReadByte();

            _handlerInvoker.Invoke(packetHeader, this, packet);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occured while processing core packet.");
        }

        return Task.CompletedTask;
    }

    protected override void OnConnected()
    {
        _logger.LogTrace("Connected to Cluster Cache server.");
    }

    protected override void OnDisconnected()
    {
        _logger.LogTrace("Disconnected from Cluster Cache server.");
    }

    public void AuthenticateWorldServer()
    {
        using ClusterAuthenticateWorldChannelPacket packet = new(_worldOptions.Value, _worldServer.ConnectedPlayers.Count());
        Send(packet);
    }

    public void DisconnectCharacter(int characterId)
    {
        using ClusterPlayerDisconnectedPacket packet = new(characterId);
        Send(packet);
    }

    public void SendMessage<TMessage>(TMessage message) where TMessage : class
    {
        string messageValue = JsonSerializer.Serialize(message, JsonOptions);

        using CorePacket packet = new(CorePacketType.BroadcastMessage);
        packet.WriteString(messageValue);
        Send(packet);
    }

    public CoreMessage ReadMessage(string message)
    {
        return JsonSerializer.Deserialize<CoreMessage>(message, JsonOptions);
    }
}
