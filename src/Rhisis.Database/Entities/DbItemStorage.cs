using System;

namespace Rhisis.Database.Entities
{
    public class DbItemStorage
    {
        /// <summary>
        /// Gets or sets the item storage primary id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the item storage type id.
        /// </summary>
        public int StorageTypeId { get; set; }

        /// <summary>
        /// Gets or sets the item storage type.
        /// </summary>
        public DbItemStorage StorageType { get; set; }

        /// <summary>
        /// Gets or sets the character id that owns the current item.
        /// </summary>
        public int CharacterId { get; set; }

        /// <summary>
        /// Gets or sets the character that owns the current item.
        /// </summary>
        public DbCharacter Character { get; set; }

        /// <summary>
        /// Gets or sets the item id.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        public DbItem Item { get; set; }

        /// <summary>
        /// Gets or sets the item slot.
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Gets or sets the item quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the item storage last update.
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the item is deleted or not.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
