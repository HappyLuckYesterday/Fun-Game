using Rhisis.Game.Common.Resources;

namespace Rhisis.World.Game.Structures
{
    public class InventoryItem : Item
    {
        /// <summary>
        /// Gets the database storage item id.
        /// </summary>
        public int? DatabaseStorageItemId { get; private set; }

        /// <summary>
        /// Gets the current used item quantity
        /// </summary>
        public int ExtraUsed { get; set; }

        /// <summary>
        /// Creates an empty <see cref="InventoryItem."/>
        /// </summary>
        /// <remarks>
        /// This item cannot be updated after created.
        /// </remarks>
        public InventoryItem() :
            base()
        {
        }

        /// <summary>
        /// Creates a new <see cref="InventoryItem"/>.
        /// </summary>
        /// <param name="itemData"></param>
        /// <param name="quantity"></param>
        /// <param name="index"></param>
        /// <param name="slot"></param>
        /// <param name="databaseId"></param>
        public InventoryItem(ItemData itemData, int quantity, int index, int slot, int? databaseId, int? databaseStorageItemId) 
            : base(itemData, quantity, databaseId)
        {
            Index = index;
            Slot = slot;
            DatabaseStorageItemId = databaseStorageItemId;
        }

        public override void CopyFrom(Item itemToCopy)
        {
            if (itemToCopy is InventoryItem inventoryItem)
            {
                DatabaseStorageItemId = inventoryItem.DatabaseStorageItemId;
            }

            base.CopyFrom(itemToCopy);
        }

        public override void Reset()
        {
            ExtraUsed = 0;
            DatabaseStorageItemId = null;

            base.Reset();
        }
    }
}