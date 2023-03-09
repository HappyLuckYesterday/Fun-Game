using Microsoft.Extensions.Logging;
using Rhisis.Protocol;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Rhisis.ClusterServer;

public sealed class ClusterUser : FFUserConnection
{
    private readonly IServiceProvider _serviceProvider;

    public ClusterUser(ILogger<ClusterUser> logger, IServiceProvider serviceProvider) 
        : base(logger)
    {
        _serviceProvider = serviceProvider;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        if (Socket is null)
        {
            Logger.LogTrace("Skip to handle login packet. Reason: client is not connected.");
            return Task.CompletedTask;
        }

        try
        {
            // We must skip the first 4 bytes because it represents the DPID which is always 0xFFFFFFFF (uint.MaxValue)
            byte[] packetBufferArray = packetBuffer.Skip(4).ToArray();
            FFPacket packet = new(packetBufferArray);
            PacketDispatcher.Execute(this, packet.Header, packet, _serviceProvider);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occured while handling a cluster packet.");
        }

        return base.HandleMessageAsync(packetBuffer);
    }
}
