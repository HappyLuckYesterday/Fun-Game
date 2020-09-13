using Rhisis.Core.Extensions;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Sylver.Network.Data;
using System;
using System.Diagnostics;

namespace Rhisis.World.Game.Structures
{
    /// <summary>
    /// FlyFF item structure.
    /// </summary>
    [DebuggerDisplay("{Name} +{Refine} ({Element}+{ElementRefine}) (x{Quantity})")]
    public class Item
    {
        public const int Empty = -1;

        private int _creatorId;
        private int _quantity;
        private byte _refine;
        private ElementType _elementType;
        private byte _elementRefine;

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public int Id => Data?.Id ?? Empty;

        /// <summary>
        /// Gets the item name.
        /// </summary>
        public string Name => Data?.Name ?? "Undefined";

        /// <summary>
        /// Gets the item data informations.
        /// </summary>
        public ItemData Data { get; private set; }

        /// <summary>
        /// Gets the item database id.
        /// </summary>
        public int? DatabaseItemId { get; private set; }

        /// <summary>
        /// Gets the creator id of the item.
        /// </summary>
        public int CreatorId
        {
            get => _creatorId;
            set
            {
                if (Data == null)
                {
                    throw new InvalidOperationException("Cannot update creator id of an empty id.");
                }

                _creatorId = value;
            }
        }

        /// <summary>
        /// Gets the item quantity.
        /// </summary>
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (Data == null)
                {
                    throw new InvalidOperationException("Cannot update quantity of an empty item.");
                }

                _quantity = value;
            }
        }

        /// <summary>
        /// Gets the item unique Id.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets the item current slot.
        /// </summary>
        public int Slot { get; set; }

        /// <summary>
        /// Gets or sets the item refine.
        /// </summary>
        public byte Refine
        {
            get => _refine;
            set
            {
                if (Data == null)
                {
                    throw new InvalidOperationException("Cannot update refine option of an empty item.");
                }

                _refine = value;
            }
        }

        /// <summary>
        /// Gets or sets the item element.
        /// </summary>
        public ElementType Element
        {
            get => _elementType;
            set
            {
                if (Data == null)
                {
                    throw new InvalidOperationException("Cannot update refine element type of an empty item.");
                }

                _elementType = value;
            }
        }

        /// <summary>
        /// Gets or sets the item element refine.
        /// </summary>
        public byte ElementRefine
        {
            get => _elementRefine;
            set
            {
                if (Data == null)
                {
                    throw new InvalidOperationException("Cannot update refine element option of an empty item.");
                }

                _elementRefine = value;
            }
        }

        /// <summary>
        /// Gets the items refine options.
        /// </summary>
        public int Refines => Refine & (byte)Element & ElementRefine;

        public Item()
        {
        }

        public Item(ItemData itemData, int quantity, int? databaseId)
        {
            if (itemData == null)
            {
                throw new ArgumentNullException(nameof(itemData));
            }

            if (quantity < 0)
            {
                throw new ArgumentException("Item id cannot be negative.");
            }

            if (quantity > itemData.PackMax)
            {
                throw new InvalidOperationException($"Cannot create {quantity} {Data.Name} because it exceeds the item maximum pack size.");
            }

            Data = itemData;
            DatabaseItemId = databaseId.GetValueOrDefault(0);
            _quantity = quantity;
            Slot = Empty;
            Index = Empty;
        }

        /// <summary>
        /// Serialize the item into the packet.
        /// </summary>
        /// <param name="packet"></param>
        public virtual void Serialize(INetPacketStream packet, int itemIndex)
        {
            packet.Write(itemIndex);
            packet.Write(Id);
            packet.Write(0); // Serial number
            packet.Write(Data?.Name.TakeCharacters(31) ?? "[undefined]");
            packet.Write((short)Quantity);
            packet.Write<byte>(0); // Repair number
            packet.Write(0); // Hp
            packet.Write(0); // Repair
            packet.Write<byte>(0); // flag ?
            packet.Write((int)Refine);
            packet.Write(0); // guild id (cloaks?)
            packet.Write((byte)Element);
            packet.Write((int)ElementRefine);
            packet.Write(0); // m_nResistSMItemId
            packet.Write(0); // Piercing size
            packet.Write(0); // Ultimate piercing size
            packet.Write(0); // Pet vis
            packet.Write(0); // charged
            packet.Write<long>(0); // m_iRandomOptItemId
            packet.Write(0); // m_dwKeepTime
            packet.Write<byte>(0); // pet
            packet.Write(0); // m_bTranformVisPet
        }

        /// <summary>
        /// Clones this <see cref="Item"/>.
        /// </summary>
        /// <returns></returns>
        public virtual Item Clone()
        {
            return new Item(Data, Quantity, DatabaseItemId)
            {
                CreatorId = CreatorId,
                Refine = Refine,
                Element = Element,
                ElementRefine = ElementRefine,
            };
        }

        public virtual void CopyFrom(Item itemToCopy)
        {
            _creatorId = itemToCopy.CreatorId;
            _refine = itemToCopy.Refine;
            _elementType = itemToCopy.Element;
            _elementRefine = itemToCopy.ElementRefine;
            _quantity = itemToCopy.Quantity;
            DatabaseItemId = itemToCopy.DatabaseItemId;
            Data = itemToCopy.Data;
        }

        /// <summary>
        /// Reset the item.
        /// </summary>
        public virtual void Reset()
        {
            DatabaseItemId = null;
            Quantity = 0;
            CreatorId = Empty;
            Refine = 0;
            Element = 0;
            ElementRefine = 0;
            Data = null;
        }

        public override string ToString() => Name;
    }
}