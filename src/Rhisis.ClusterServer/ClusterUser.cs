using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Helpers;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Server;
using Sylver.HandlerInvoker;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer;

public sealed class ClusterUser : FFUserConnection, IClusterUser
{
    private readonly IHandlerInvoker _handlerInvoker;
    private readonly IClusterServer _server;

    public int UserId { get; set; }

    public string Username { get; set; }

    public int LoginProtectValue { get; set; } = new Random().Next(0, 1000);

    /// <summary>
    /// Creates a new <see cref="ClusterUser"/> instance.
    /// </summary>
    /// <param name="server">Cluster server.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="handlerInvoker">Handler invoker.</param>
    public ClusterUser(IClusterServer server, ILogger<ClusterUser> logger, IHandlerInvoker handlerInvoker)
        : base(logger)
    {
        _server = server;
        _handlerInvoker = handlerInvoker;
    }

    public void Disconnect() => _server.DisconnectUser(Id);

    /// <summary>
    /// Handle the incoming mesages.
    /// </summary>
    /// <param name="packet">Incoming packet</param>
    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        uint packetHeaderNumber = 0;

        if (Socket is null)
        {
            Logger.LogTrace("Skip to handle cluster packet from null socket. Reason: client is not connected.");
            return Task.CompletedTask;
        }

        try
        {
            using var packet = new FFPacket(packetBuffer);

            packet.ReadUInt32(); // DPID: Always 0xFFFFFFFF (uint.MaxValue)
            packetHeaderNumber = packet.ReadUInt32();

#if DEBUG
            Logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, Socket.RemoteEndPoint);
#endif
            _handlerInvoker.Invoke((PacketType)packetHeaderNumber, this, packet);
        }
        catch (ArgumentNullException)
        {
            if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
            {
                Logger.LogTrace("Received an unimplemented Cluster packet {0} (0x{1}) from {2}.",
                    Enum.GetName(typeof(PacketType), packetHeaderNumber),
                    packetHeaderNumber.ToString("X4"),
                    Socket.RemoteEndPoint);
            }
            else
            {
                Logger.LogTrace("[SECURITY] Received an unknown Cluster packet 0x{0} from {1}.",
                    packetHeaderNumber.ToString("X4"),
                    Socket.RemoteEndPoint);
            }
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, $"An error occured while handling a cluster packet.");
            Logger.LogDebug(exception.InnerException?.StackTrace);
        }

        return Task.CompletedTask;
    }

    protected override void OnConnected()
    {
        Logger.LogInformation($"New client connected to cluster server from {Socket.RemoteEndPoint}.");

        using var welcomePacket = new WelcomePacket(SessionId);
        Send(welcomePacket);
    }

    protected override void OnDisconnected()
    {
        Logger.LogInformation($"Client disconnected from {Socket?.RemoteEndPoint.ToString() ?? "unknown location"}.");
    }
}
