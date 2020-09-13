using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Sylver.Network.Data;

namespace Rhisis.Game.Abstractions
{
    public interface IItem : IPacketSerializer
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the item database storage id.
        /// </summary>
        int? DatabaseStorageItemId { get; }

        /// <summary>
        /// Gets the item name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the item data.
        /// </summary>
        ItemData Data { get; }

        /// <summary>
        /// Gets the item creator id.
        /// </summary>
        int CreatorId { get; }

        /// <summary>
        /// Gets or sets the item quantity.
        /// </summary>
        int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the item index inside its storage.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets the item slot inside its storage.
        /// </summary>
        int Slot { get; set; }

        /// <summary>
        /// Gets or sets the item refine.
        /// </summary>
        byte Refine { get; set; }

        /// <summary>
        /// Gets or sets the item element type.
        /// </summary>
        ElementType Element { get; set; }

        /// <summary>
        /// Gets or sets the item element refine.
        /// </summary>
        byte ElementRefine { get; set; }

        /// <summary>
        /// Gets the item refines.
        /// </summary>
        int Refines { get; }

        /// <summary>
        /// Copy the given item information in to the current one.
        /// </summary>
        /// <param name="item">Item to copy.</param>
        void CopyFrom(IItem item);

        /// <summary>
        /// Serialize the current item with a custom storage index.
        /// </summary>
        /// <param name="packet">Current packet stream.</param>
        /// <param name="itemIndex">Item storage index.</param>
        void Serialize(INetPacketStream packet, int itemIndex);
    }
}
