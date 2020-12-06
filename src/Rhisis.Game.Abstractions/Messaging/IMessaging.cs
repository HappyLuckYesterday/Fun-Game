using System;

namespace Rhisis.Game.Abstractions.Messaging
{
    /// <summary>
    /// Provides an interface to send messages using a message queue system.
    /// </summary>
    public interface IMessaging : IDisposable
    {
        /// <summary>
        /// Disconnected event fired when the current client losses connection.
        /// </summary>
        event EventHandler Disconnected;

        /// <summary>
        /// Publishes a new message into the given messaging queue name named as the given message type name.
        /// </summary>
        /// <typeparam name="TMessage">Message type.</typeparam>
        /// <param name="message">Message to publish.</param>
        /// <remarks>
        /// The given object message will be serialized as JSON and sent as a byte array.
        /// </remarks>
        void Publish<TMessage>(TMessage message) where TMessage : class;

        /// <summary>
        /// Publishes a new message into the given messaging queue name.
        /// </summary>
        /// <typeparam name="TMessage">Message type.</typeparam>
        /// <param name="name">Messaging queue name.</param>
        /// <param name="message">Message to publish.</param>
        /// <remarks>
        /// The given object message will be serialized as JSON and sent as a byte array.
        /// </remarks>
        void Publish<TMessage>(string name, TMessage message) where TMessage : class;

        /// <summary>
        /// Subscribe to an event queue named as the given message type name.
        /// </summary>
        /// <typeparam name="TMessage">Message type.</typeparam>
        /// <param name="callback">Action to execute every time a message is received.</param>
        void Subscribe<TMessage>(Action<TMessage> callback) where TMessage : class;

        /// <summary>
        /// Subscribe to an event queue.
        /// </summary>
        /// <typeparam name="TMessage">Message type.</typeparam>
        /// <param name="name">Message queue name.</param>
        /// <param name="callback">Action to execute every time a message is received.</param>
        void Subscribe<TMessage>(string name, Action<TMessage> callback) where TMessage : class;
    }
}
