using Microsoft.Extensions.Logging;
using Rhisis.Game.Entities;
using Rhisis.Protocol;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rhisis.WorldServer;

public sealed class WorldUser : FFUserConnection
{
    private readonly IServiceProvider _serviceProvider;

    internal Player Player { get; set; }

    public WorldUser(ILogger<WorldUser> logger, IServiceProvider serviceProvider) 
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
            Logger.LogError(e, "An error occured while handling a login packet.");
        }

        return base.HandleMessageAsync(packetBuffer);
    }

    protected override void OnDisconnected()
    {
        // TODO: save player to database
        // TODO: notify cluster and disconnect from messenger

        Player.Dispose();
        Player = null;

        base.OnDisconnected();
    }
}
