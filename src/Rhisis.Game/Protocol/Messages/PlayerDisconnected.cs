namespace Rhisis.Game.Protocol.Messages
{
    /// <summary>
    /// Provides a data structure that represents a message when a player gets disconnected.
    /// </summary>
    public class PlayerDisconnected
    {
        /// <summary>
        /// Gets or sets the player id that has disconnected.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="PlayerDisconnected"/> message.
        /// </summary>
        public PlayerDisconnected()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PlayerDisconnected"/> message.
        /// </summary>
        /// <param name="playerId">Player id.</param>
        public PlayerDisconnected(int playerId)
        {
            Id = playerId;
        }
    }
}
