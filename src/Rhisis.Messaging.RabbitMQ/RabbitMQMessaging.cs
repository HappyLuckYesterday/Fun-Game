using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Rhisis.Messaging.Abstractions;
using System;
using System.Text;
using System.Text.Json;

namespace Rhisis.Messaging.RabbitMQ
{
    internal sealed class RabbitMQMessaging : IMessaging
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public event EventHandler? Disconnected;

        public RabbitMQMessaging(RabbitMQBuilderOptions options)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = options.Host,
                UserName = options.Username,
                Password = options.Password,
                VirtualHost = "/"
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _connection.ConnectionShutdown += OnDisconnected;
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class
        {
            Publish(typeof(TMessage).Name, message);
        }

        public void Publish<TMessage>(string name, TMessage message) where TMessage : class
        {
            _channel.ExchangeDeclare(exchange: name, type: ExchangeType.Fanout);

            string content = JsonSerializer.Serialize(message);
            byte[] contentData = Encoding.UTF8.GetBytes(content);

            _channel.BasicPublish(exchange: name, routingKey: "", basicProperties: null, body: contentData);
        }

        public void Subscribe<TMessage>(Action<TMessage> callback) where TMessage : class
        {
            Subscribe(typeof(TMessage).Name, callback);
        }

        public void Subscribe<TMessage>(string name, Action<TMessage> callback) where TMessage : class
        {
            _channel.ExchangeDeclare(exchange: name, type: ExchangeType.Fanout);
            
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: name, routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, deliverEvent) =>
            {
                if (callback is not null)
                {
                    try
                    {
                        var messageContent = Encoding.UTF8.GetString(deliverEvent.Body.ToArray());
                        TMessage message = JsonSerializer.Deserialize<TMessage>(messageContent);

                        callback(message);
                    }
                    catch (Exception e)
                    {
                    }
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        private void OnDisconnected(object sender, ShutdownEventArgs e)
        {
            Disconnected?.Invoke(this, null);
        }
    }
}
