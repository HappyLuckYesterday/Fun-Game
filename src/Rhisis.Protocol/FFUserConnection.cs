using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Protocol.Packets;
using System;

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

    protected override void OnConnected()
    {
        Logger.LogInformation($"New user connected (SessionId={SessionId}|Id={Id})");
        using WelcomePacket packet = new(SessionId);

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
