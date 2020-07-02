using Rhisis.Core.Structures.Game;

namespace Rhisis.World.Game.Structures
{
    public class InventoryItem : Item
    {
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
        public InventoryItem(ItemData itemData, int quantity, int index, int slot, int? databaseId) 
            : base(itemData, quantity, databaseId)
        {
            Index = index;
            Slot = slot;
        }
    }
}