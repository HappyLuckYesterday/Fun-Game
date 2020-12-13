namespace Rhisis.Game.Protocol.Messages
{
    public class PlayerMessengerMessage
    {
        /// <summary>
        /// Gets or sets the player id that is sending the message.
        /// </summary>
        public int FromId { get; set; }

        /// <summary>
        /// Gets or sets the player name that is sending the message.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the player id that will receive the message.
        /// </summary>
        public int DestinationId { get; set; }

        /// <summary>
        /// Gets or sets the message to be sent.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates a new <see cref="PlayerMessengerMessage"/> instance.
        /// </summary>
        public PlayerMessengerMessage()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PlayerMessengerMessage"/> instance.
        /// </summary>
        /// <param name="fromId">Player id sending the message.</param>
        /// <param name="destinationId">Player name sending the message.</param>
        /// <param name="destinationId">Player id receiving the message.</param>
        /// <param name="message">Message to be sent.</param>
        public PlayerMessengerMessage(int fromId, string fromName, int destinationId, string message)
        {
            FromId = fromId;
            FromName = fromName;
            DestinationId = destinationId;
            Message = message;
        }
    }
}
