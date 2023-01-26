using LiteMessageHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.LoginServer.Handlers;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.Threading.Tasks;

namespace Rhisis.LoginServer;

internal sealed class LoginUser : FFUserConnection
{
    private readonly IMessageHandlerDispatcher _messageDispatcher;
    private readonly IServiceProvider _serviceProvider;

    public Guid UserId { get; set; }

    public string Username { get; set; }

    public LoginUser(ILogger<LoginUser> logger, IMessageHandlerDispatcher messageDispatcher, IServiceProvider serviceProvider) 
        : base(logger)
    {
        _messageDispatcher = messageDispatcher;
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
            FFPacket packet = new(packetBuffer);
            Type packetType = PacketHandlerCache.GetPacketType(packet.Header);
            MessageHandler packetHandler = GetHandler(packetType);
            LoginPacketHandler packetHandlerMessage = CreatePacketHandlerMessage(packetType, packet);

            packetHandler.Execute(packetHandlerMessage);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occured while handling a login packet.");
        }

        return base.HandleMessageAsync(packetBuffer);
    }

    protected override void OnConnected()
    {
        base.OnConnected();
    }

    protected override void OnError(object sender, Exception exception)
    {

        base.OnError(sender, exception);
    }

    private MessageHandler GetHandler(Type packetType)
    {
        MessageHandler packetHandler = _messageDispatcher.GetHandler(packetType);

        if (packetHandler is null)
        {
            throw new ArgumentException($"Failed to find handler for packet type: {packetType.Name}");
        }

        if (packetHandler.Target is LoginPacketHandler loginHandler)
        {
            loginHandler.User = this;
        }

        return packetHandler;
    }

    private LoginPacketHandler CreatePacketHandlerMessage(Type packetType, FFPacket packet)
    {
        var packetObject = ActivatorUtilities.CreateInstance(_serviceProvider, packetType, packet) as LoginPacketHandler;

        if (packetObject is not null)
        {
            packetObject.User = this;
        }

        return packetObject;
    }
}
