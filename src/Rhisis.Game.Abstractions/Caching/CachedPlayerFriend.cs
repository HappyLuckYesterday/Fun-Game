namespace Rhisis.Game.Abstractions.Caching
{
    /// <summary>
    /// Provides a data-structure representing a cached friend.
    /// </summary>
    public class CachedPlayerFriend
    {
        /// <summary>
        /// Gets or sets the player's friend id.
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        /// Gets or sets a boolean that indicates if the friend is blocked.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Creates a new <see cref="CachedPlayerFriend"/> empty instance.
        /// </summary>
        public CachedPlayerFriend()
        {
        }

        /// <summary>
        /// Creates a new <see cref="CachedPlayerFriend"/> instance.
        /// </summary>
        /// <param name="friendId">Friend id.</param>
        /// <param name="blocked">Blocked state.</param>
        public CachedPlayerFriend(int friendId, bool blocked = false)
        {
            FriendId = friendId;
            IsBlocked = blocked;
        }
    }
}