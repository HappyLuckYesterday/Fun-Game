using LiteMessageHandler;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Protocol;
using Rhisis.Protocol.Exceptions;
using Rhisis.Protocol.Handlers;
using System;
using System.Threading.Tasks;

namespace Rhisis.LoginServer;

internal sealed class LoginUser : FFUserConnection
{
    private readonly IMessageHandlerDispatcher _messageDispatcher;
    private readonly LoginServer _loginServer;

    public Guid UserId { get; set; }

    public string Username { get; set; }

    public LoginUser(ILogger<LoginUser> logger, IMessageHandlerDispatcher messageDispatcher, LoginServer loginServer) 
        : base(logger)
    {
        _messageDispatcher = messageDispatcher;
        _loginServer = loginServer;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        if (Socket is null)
        {
            Logger.LogTrace("Skip to handle login packet. Reason: client is not connected.");
            return Task.CompletedTask;
        }

        MessageHandler packetHandler = null;

        try
        {
            FFPacket packet = new(packetBuffer);
            Type packetType = PacketHandlerCache.GetPacketType(packet.Header);

            if (packetType is null)
            {
                throw new PacketHandlerNotImplemented(packet.Header);
            }

            object packetHandlerMessage = Activator.CreateInstance(packetType, packet);

            packetHandler = GetHandler(packetType);
            packetHandler.Execute(packetHandlerMessage);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occured while handling a login packet.");
        }
        finally
        {
            packetHandler?.Dispose();
        }

        return base.HandleMessageAsync(packetBuffer);
    }

    public void Disconnect(string reason = null)
    {
        _loginServer.DisconnectUser(Id);

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Logger.LogInformation($"{Username} disconnected. Reason: {reason}");
        }
    }

    private MessageHandler GetHandler(Type packetType)
    {
        MessageHandler packetHandler = _messageDispatcher.GetHandler(packetType);

        if (packetHandler is null)
        {
            throw new ArgumentException($"Failed to find handler for packet type: {packetType.Name}");
        }

        packetHandler.Target.AssignProperty("User", this);
        packetHandler.Target.AssignProperty("Server", _loginServer);

        return packetHandler;
    }
}
