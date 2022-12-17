using LiteNetwork.Client;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker;
using System;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer.Core;

public class ClusterCoreClient : LiteClient, ICoreClient
{
    private readonly ILogger<ClusterCoreClient> _logger;
    private readonly IHandlerInvoker _handlerInvoker;

    public ClusterCoreClient(LiteClientOptions options, 
        ILogger<ClusterCoreClient> logger, 
        IHandlerInvoker handlerInvoker, 
        IServiceProvider serviceProvider) 
        : base(options, serviceProvider)
    {
        _logger = logger;
        _handlerInvoker = handlerInvoker;
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

    public void UpdateWorldChannel(WorldChannel channel)
    {
        using var packet = new CorePacket();

        packet.WriteByte((byte)CorePacketType.UpdateClusterWorldChannel);
        packet.WriteByte((byte)channel.Id);
        packet.WriteString(channel.Name);
        packet.WriteString(channel.Host);
        packet.WriteUInt16((ushort)channel.Port);
        packet.WriteInt32(channel.ConnectedUsers);
        packet.WriteInt32(channel.MaximumUsers);

        Send(packet);
    }

    public void RemoveWorldChannel(WorldChannel channel)
    {
        using var packet = new CorePacket();

        packet.WriteByte((byte)CorePacketType.RemoveClusterWorldChannel);
        packet.WriteByte((byte)channel.Id);

        Send(packet);
    }
}
