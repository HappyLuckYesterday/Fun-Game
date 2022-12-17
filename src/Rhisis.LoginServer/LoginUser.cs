using Microsoft.Extensions.Logging;
using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Server;
using Sylver.HandlerInvoker;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rhisis.LoginServer;

public sealed class LoginUser : FFUserConnection, ILoginUser
{
    private const string UnknownUsername = "UNKNOWN";

    private readonly ILoginServer _server;
    private readonly IHandlerInvoker _handlerInvoker;

    public int UserId { get; private set; }

    public string Username { get; private set; } = UnknownUsername;

    public bool IsConnected => !string.IsNullOrEmpty(Username);

    /// <summary>
    /// Creates a new <see cref="LoginUser"/> instance.
    /// </summary>
    /// <param name="server">Login Server.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="handlerInvoker">Handler invoker.</param>
    public LoginUser(ILoginServer server, ILogger<LoginUser> logger, IHandlerInvoker handlerInvoker)
        : base(logger)
    {
        _server = server;
        _handlerInvoker = handlerInvoker;
    }

    public void Disconnect() => Disconnect(null);

    public void Disconnect(string reason)
    {
        _server.DisconnectUser(Id);

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Logger.LogInformation($"{Username} disconnected. Reason: {reason}");
        }
    }

    public void SetClientUsername(string username, int userId)
    {
        if (!Username.Equals(UnknownUsername, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Client username already set.");
        }

        Username = username;
        UserId = userId;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        uint packetHeaderNumber = 0;

        if (Socket is null)
        {
            Logger.LogTrace("Skip to handle login packet. Reason: client is not connected.");
            return Task.CompletedTask;
        }

        try
        {
            using var packet = new FFPacket(packetBuffer);

            packetHeaderNumber = packet.ReadUInt32();
            var packetType = (PacketType)packetHeaderNumber;

            Logger.LogTrace($"Received {packetType} (0x{packetHeaderNumber:X2}) packet from {Socket.RemoteEndPoint}.");
            _handlerInvoker.Invoke(packetType, this, packet);
        }
        catch (ArgumentException)
        {
            if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
            {
                string packetName = Enum.GetName(typeof(PacketType), packetHeaderNumber);

                Logger.LogTrace($"Received an unimplemented Login packet {packetName} (0x{packetHeaderNumber:X2}) from {Socket.RemoteEndPoint}.");
            }
            else
            {
                Logger.LogTrace($"Received an unknown Login packet 0x{packetHeaderNumber:X2} from {Socket.RemoteEndPoint}.");
            }
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "An error occured while handling a login packet.");
        }

        return Task.CompletedTask;
    }

    protected override void OnConnected()
    {
        Logger.LogInformation($"New client connected from {Socket.RemoteEndPoint}.");

        using var welcomePacket = new WelcomePacket(SessionId);
        Send(welcomePacket);
    }

    protected override void OnDisconnected()
    {
        Logger.LogInformation($"Client '{Username}' disconnected from {Socket.RemoteEndPoint}.");
    }
}