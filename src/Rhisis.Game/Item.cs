using Rhisis.Core.Extensions;
using Rhisis.Abstractions;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using System;
using System.Diagnostics;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Game
{
    [DebuggerDisplay("{Name} +{Refine} ({Element}+{ElementRefine}) (x{Quantity})")]
    public class Item : IItem
    {
        private int _creatorId;
        private int _quantity;
        private byte _refine;
        private ElementType _elementType;
        private byte _elementRefine;

        public int Id => Data?.Id ?? -1;

        public int? DatabaseStorageItemId { get; private set; }

        public string Name => Data?.Name ?? "undefined";

        public ItemData Data { get; private set; }

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

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (Data == null)
                {
                    throw new InvalidOperationException("Cannot update quantity of an empty item.");
                }

                _quantity = Math.Clamp(value, 0, Data.PackMax);
            }
        }

        public int Index { get; set; }

        public int Slot { get; set; }

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

        public int Refines => Refine & (byte)Element & ElementRefine;

        public Item()
        {
            Index = -1;
            Slot = -1;
        }

        public Item(ItemData itemData, int? quantity = null, int? databaseStorageId = null)
        {
            Data = itemData ?? throw new ArgumentNullException(nameof(itemData));
            DatabaseStorageItemId = databaseStorageId;
            Index = -1;
            Slot = -1;
            _quantity = quantity.GetValueOrDefault();
        }

        public virtual void CopyFrom(IItem itemToCopy)
        {
            DatabaseStorageItemId = itemToCopy.DatabaseStorageItemId;
            Data = itemToCopy.Data;
            _creatorId = itemToCopy.CreatorId;
            _refine = itemToCopy.Refine;
            _elementType = itemToCopy.Element;
            _elementRefine = itemToCopy.ElementRefine;
            _quantity = itemToCopy.Quantity;
        }

        public virtual void Reset()
        {
            DatabaseStorageItemId = null;
            Quantity = 0;
            CreatorId = -1;
            Refine = 0;
            Element = 0;
            ElementRefine = 0;
            Data = null;
        }

        public void Serialize(IFFPacket packet, int itemIndex)
        {
            packet.WriteInt32(itemIndex);
            packet.WriteInt32(Id);
            packet.WriteInt32(0); // Serial number
            packet.WriteString(Name.TakeCharacters(31));
            packet.WriteInt16((short)Quantity);
            packet.WriteByte(0); // Repair number
            packet.WriteInt32(0); // Hp
            packet.WriteInt32(0); // Repair
            packet.WriteByte(0); // flag ?
            packet.WriteInt32((int)Refine);
            packet.WriteInt32(0); // guild id (cloaks?)
            packet.WriteByte((byte)Element);
            packet.WriteInt32((int)ElementRefine);
            packet.WriteInt32(0); // m_nResistSMItemId
            packet.WriteInt32(0); // Piercing size
            packet.WriteInt32(0); // Ultimate piercing size
            packet.WriteInt32(0); // Pet vis
            packet.WriteInt32(0); // charged
            packet.WriteInt64(0); // m_iRandomOptItemId
            packet.WriteInt32(0); // m_dwKeepTime
            packet.WriteByte(0); // pet
            packet.WriteInt32(0); // m_bTranformVisPet
        }

        public void Serialize(IFFPacket packet) => Serialize(packet, Index);

        public IItem Clone()
        {
            return new Item(Data)
            {
                Quantity = Quantity,
                Element = Element,
                ElementRefine = ElementRefine,
                Refine = Refine
            };
        }
    }
}
