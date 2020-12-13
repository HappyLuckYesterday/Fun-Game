namespace Rhisis.Game.Protocol.Messages
{
    /// <summary>
    /// Provides a data structure used to notify that a player has blocked a friend.
    /// </summary>
    public class PlayerMessengerBlockFriend
    {
        /// <summary>
        /// Gets or sets the player id that is blocking the friend.
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the player friend id being blocked by another player.
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        /// Creates a new <see cref="PlayerMessengerBlockFriend"/> instance.
        /// </summary>
        public PlayerMessengerBlockFriend()
        {
        }

        /// <summary>
        /// Creates a new <see cref="PlayerMessengerBlockFriend"/> instance.
        /// </summary>
        /// <param name="playerId">Player id blocking the friend.</param>
        /// <param name="friendId">Friend id being blocked.</param>
        public PlayerMessengerBlockFriend(int playerId, int friendId)
        {
            PlayerId = playerId;
            FriendId = friendId;
        }
    }
}
