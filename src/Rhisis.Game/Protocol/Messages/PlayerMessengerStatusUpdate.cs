using Rhisis.Game.Common;

namespace Rhisis.Game.Protocol.Messages
{
    /// <summary>
    /// Provides a data structure that notifies that a player's status has been updated.
    /// </summary>
    public class PlayerMessengerStatusUpdate
    {
        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the player messenger status.
        /// </summary>
        public MessengerStatusType Status { get; set; }

        /// <summary>
        /// Creates a new <see cref="PlayerMessengerStatusUpdate"/> message.
        /// </summary>
        public PlayerMessengerStatusUpdate()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PlayerMessengerStatusUpdate"/> message.
        /// </summary>
        /// <param name="playerId">Player ID.</param>
        /// <param name="statusType">Messenger status.</param>
        public PlayerMessengerStatusUpdate(int playerId, MessengerStatusType statusType)
        {
            Id = playerId;
            Status = statusType; 
        }
    }
}
