namespace Rhisis.Protocol.Networking;

public interface IFFInterServerConnectionHandler<in TUser, TMessage>
{
    void Execute(TUser user, TMessage message);
}
