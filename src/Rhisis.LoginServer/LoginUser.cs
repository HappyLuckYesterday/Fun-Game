using Microsoft.Extensions.Logging;
using Rhisis.Protocol;
using System;
using System.Threading.Tasks;

namespace Rhisis.LoginServer;

public sealed class LoginUser : FFUserConnection
{
    private readonly LoginServer _loginServer;
    private readonly IServiceProvider _serviceProvider;

    public int UserId { get; set; }

    public string Username { get; set; }

    public LoginUser(ILogger<LoginUser> logger, LoginServer loginServer, IServiceProvider serviceProvider) 
        : base(logger)
    {
        _loginServer = loginServer;
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
            PacketDispatcher.Execute(this, packet.Header, packet, _serviceProvider);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occured while handling a login packet.");
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
}
