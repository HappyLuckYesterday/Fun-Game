using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.TaskbarPlayer;

public class Shortcut : IPacketSerializer
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
    public void Serialize(FFPacket packet)
    {
        packet.WriteInt32(Slot);
        packet.WriteUInt32((uint)Type);
        packet.WriteUInt32((uint)ItemIndex.GetValueOrDefault());
        packet.WriteUInt32((uint)ObjectType);
        packet.WriteUInt32(ObjectIndex);
        packet.WriteUInt32(UserId);
        packet.WriteUInt32(ObjectData);

        if (Type == ShortcutType.Chat)
        {
            packet.WriteString(Text);
        }
    }
}