using LiteMessageHandler;

namespace Rhisis.LoginServer.Handlers;

internal abstract class LoginPacketHandler<TMessage> : IMessageHandler<TMessage>
{
    public LoginUser User { get; internal set; }

    public LoginServer Server { get; internal set; }

    protected LoginPacketHandler()
    {
    }

    public abstract void Execute(TMessage message);
}