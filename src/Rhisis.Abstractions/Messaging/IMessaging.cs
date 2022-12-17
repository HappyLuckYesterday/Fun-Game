namespace Rhisis.Abstractions.Messaging;

public interface IMessaging
{
    void SendMessage<TMessage>(TMessage message) where TMessage : class;
}
