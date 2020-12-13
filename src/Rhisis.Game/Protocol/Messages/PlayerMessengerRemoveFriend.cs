namespace Rhisis.Game.Protocol.Messages
{
    /// <summary>
    /// Provides a data structure that notifies the removed friend id that the player has removed him
    /// from its friends list.
    /// </summary>
    public class PlayerMessengerRemoveFriend
    {
        /// <summary>
        /// Gets or sets the player's id that is removing the friend.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the removed friend's id.
        /// </summary>
        public int RemovedFriendId { get; set; }

        /// <summary>
        /// Creates a new <see cref="PlayerMessengerRemoveFriend"/> instance.
        /// </summary>
        public PlayerMessengerRemoveFriend()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PlayerMessengerRemoveFriend"/> instance.
        /// </summary>
        /// <param name="playerId">Player id.</param>
        /// <param name="removedFriendId">Removed friend id.</param>
        public PlayerMessengerRemoveFriend(int playerId, int removedFriendId)
        {
            PlayerId = playerId;
            RemovedFriendId = removedFriendId;
        }
    }
}
