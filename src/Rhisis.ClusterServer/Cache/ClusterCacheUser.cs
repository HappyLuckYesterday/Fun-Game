using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Cache;

public class ClusterCacheUser : LiteServerUser
{
    private readonly ILogger<ClusterCacheUser> _logger;
    private readonly IHandlerInvoker _handlerInvoker;
    private readonly ICoreClient _coreClient;

    public WorldChannel Channel { get; } = new WorldChannel();

    public ClusterCacheUser(ILogger<ClusterCacheUser> logger, IHandlerInvoker handlerInvoker, ICoreClient coreClient)
    {
        _logger = logger;
        _handlerInvoker = handlerInvoker;
        _coreClient = coreClient;
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
        _logger.LogTrace($"New incoming server connection from {Socket.RemoteEndPoint}.");
        SendWelcome();
    }

    protected override void OnDisconnected()
    {
        _logger.LogTrace($"World channel '{Channel.Name}' disconnected.");
        _coreClient.RemoveWorldChannel(Channel);
    }

    private void SendWelcome()
    {
        using var packet = new CorePacket();

        packet.WriteByte((byte)CorePacketType.Welcome);

        Send(packet);
    }
}
