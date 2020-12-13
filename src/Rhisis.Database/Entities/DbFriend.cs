namespace Rhisis.Database.Entities
{
    /// <summary>
    /// Database entity describing a friend bound.
    /// </summary>
    public class DbFriend
    {
        /// <summary>
        /// Gets or sets the character id.
        /// </summary>
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the character instance.
        /// </summary>
        public DbCharacter Character { get; set; }

        /// <summary>
        /// Gets or sets the character id of the friend.
        /// </summary>
        public int FriendId { get; set; }

        /// <summary>
        /// Gets or sets the character instance of the friend.
        /// </summary>
        public DbCharacter Friend { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the friend is blocked.
        /// </summary>
        public bool IsBlocked { get; set; }
    }
}
