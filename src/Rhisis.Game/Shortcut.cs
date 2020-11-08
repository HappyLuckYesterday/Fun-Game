using Rhisis.Core.Common;
using Rhisis.Game.Abstractions;
using Sylver.Network.Data;

namespace Rhisis.Game
{
    public class Shortcut : IShortcut
    {
        public int Slot { get; }

        public ShortcutType Type { get; }

        public int? ItemIndex { get; }

        public ShortcutObjectType ObjectType { get; }

        public uint ObjectIndex { get; }

        public uint UserId { get; }

        public uint ObjectData { get; }

        public string Text { get; }

        /// <summary>
        /// Creates a new <see cref="Shortcut"/> instance.
        /// </summary>
        /// <param name="slot">Shortcut slot in its taskbar container.</param>
        /// <param name="type">Shortcut type.</param>
        /// <param name="itemIndex">Shortcut target item index in inventory.</param>
        /// <param name="shortcutObjectType">Shortcut object type.</param>
        /// <param name="objIndex">Shortcut index in its taskbar container.</param>
        /// <param name="userId">Shortcut user id. Not used.</param>
        /// <param name="objData">Shortcut additionnal data.</param>
        /// <param name="text">Shortcut text.</param>
        public Shortcut(int slot, ShortcutType type, int? itemIndex, ShortcutObjectType shortcutObjectType, uint objIndex, uint userId, uint objData, string text)
        {
            Slot = slot;
            Type = type;
            ItemIndex = itemIndex;
            ObjectType = shortcutObjectType;
            ObjectIndex = objIndex;
            UserId = userId;
            ObjectData = objData;
            Text = text;
        }

        /// <summary>
        /// Serializes this shortcut into the give packet stream.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        public void Serialize(INetPacketStream packet)
        {
            packet.Write(Slot);
            packet.Write((uint)Type);
            packet.Write((uint)ItemIndex.GetValueOrDefault());
            packet.Write((uint)ObjectType);
            packet.Write(ObjectIndex);
            packet.Write(UserId);
            packet.Write(ObjectData);

            if (Type == ShortcutType.Chat)
            {
                packet.Write(Text);
            }
        }
    }
}