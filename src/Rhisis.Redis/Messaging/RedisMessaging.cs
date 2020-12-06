using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Redis.Internal;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace Rhisis.Redis.Messaging
{
    internal class RedisMessaging : IMessaging
    {
        public event EventHandler Disconnected;

        private readonly ConnectionMultiplexer _connection;

        public RedisMessaging(RedisConnection redisConnection)
        {
            _connection = redisConnection.Connection;
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class
        {
            Publish(typeof(TMessage).Name, message);
        }

        public void Publish<TMessage>(string name, TMessage message) where TMessage : class
        {
            _connection.GetSubscriber().Publish(name, JsonSerializer.Serialize(message));
        }

        public void Subscribe<TMessage>(Action<TMessage> callback) where TMessage : class
        {
            Subscribe(typeof(TMessage).Name, callback);
        }

        public void Subscribe<TMessage>(string name, Action<TMessage> callback) where TMessage : class
        {
            _connection.GetSubscriber().Subscribe(name, (channel, messageValue) =>
            {
                var message = JsonSerializer.Deserialize<TMessage>(messageValue);

                callback?.Invoke(message);
            });
        }

        public void Dispose()
        {
            // Nothing to do.
        }
    }
}
