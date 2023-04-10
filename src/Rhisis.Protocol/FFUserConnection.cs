using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;

namespace Rhisis.Protocol;

/// <summary>
/// Represents a FlyFF user connection.
/// </summary>
public class FFUserConnection : LiteServerUser
{
    /// <summary>
    /// Gets the user session id.
    /// </summary>
    public uint SessionId { get; } = (uint)new Random().Next(0, int.MaxValue);

    /// <summary>
    /// Gets the connection logger.
    /// </summary>
    protected ILogger Logger { get; private set; }

    protected FFUserConnection(ILogger logger)
    {
        Logger = logger;
    }

    public void Disconnect()
    {
        Dispose();
    }

    public void PacketHandlerNotImplemented(PacketType packetType)
    {
        Logger.LogWarning($"Received an unimplemented packet {packetType} (0x{(int)packetType:X8}) from {Socket.RemoteEndPoint}.");
    }

    public void SnapshotNotImplemented(SnapshotType snapshotType)
    {
        Logger.LogWarning($"Received an unimplemented snapshot {snapshotType} (0x{(int)snapshotType:X8}) from {Socket.RemoteEndPoint}.");
    }

    protected override void OnConnected()
    {
        Logger.LogInformation($"New user connected (SessionId={SessionId}|Id={Id})");
        
        using FFPacket packet = new();
        packet.WriteUInt32((uint)PacketType.WELCOME);
        packet.WriteUInt32(SessionId);

        Send(packet);
    }

    protected override void OnDisconnected()
    {
        Logger.LogInformation($"Client disconnected from {Socket?.RemoteEndPoint.ToString() ?? "unknown location"}.");
    }

    protected override void OnError(object sender, Exception exception)
    {
        Logger.LogError(exception, $"An error occured while processing a request for user '{Id}' (Session Id={SessionId})");
    }
}
