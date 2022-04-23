namespace Rhisis.Abstractions.Messaging
{
    public class Message<TContent>
    {
        /// <summary>
        /// Gets the message type.
        /// </summary>
        public MessageType Type { get; }

        /// <summary>
        /// Gets the destination player id.
        /// </summary>
        /// <remarks>
        /// If null, then it's a <see cref="MessageType.Cluster"/> message.
        /// </remarks>
        public int? PlayerId { get; }

        /// <summary>
        /// Gets the message content as object.
        /// </summary>
        public TContent Content { get; }

        /// <summary>
        /// Creates a new <see cref="Message{TContent}"/> instance, that will be broadcasted to every channels on the cluster.
        /// </summary>
        /// <param name="content">Message content.</param>
        public Message(TContent content)
        {
            Type = MessageType.Cluster;
            Content = content;
        }

        /// <summary>
        /// Creates a new <see cref="Message{TContent}"/> instance, that will be sent to the given player id, connected to a cluster channel.
        /// </summary>
        /// <param name="playerId">Destination player id.</param>
        /// <param name="content">Message content.</param>
        public Message(int playerId, TContent content)
        {
            Type = MessageType.Player;
            PlayerId = playerId;
            Content = content;
        }
    }
}
