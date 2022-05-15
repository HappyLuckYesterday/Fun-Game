using Microsoft.Extensions.DependencyInjection;
using Rhisis.Abstractions.Messaging;
using Rhisis.Core.DependencyInjection;
using Rhisis.WorldServer.Abstractions;

namespace Rhisis.WorldServer.Game.Messaging
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class Messaging : IMessaging
    {
        private readonly IClusterCacheClient _cacheClient;

        public Messaging(IClusterCacheClient cacheClient)
        {
            _cacheClient = cacheClient;
        }

        public void SendMessage<TMessage>(TMessage message) where TMessage : class
        {
            _cacheClient.SendMessage(message);
        }
    }
}
