using Rhisis.Protocol.Messages;

namespace Rhisis.WorldServer.Abstractions
{
    /// <summary>
    /// Provides a mechanism to interact with the cluster server.
    /// </summary>
    public interface IClusterCacheClient
    {
        void AuthenticateWorldServer();

        void SendMessage<TMessage>(TMessage message) where TMessage : class;

        CoreMessage ReadMessage(string message);
    }
}
