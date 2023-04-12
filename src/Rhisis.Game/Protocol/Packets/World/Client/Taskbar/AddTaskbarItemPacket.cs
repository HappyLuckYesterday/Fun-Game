using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Taskbar;

public class AddTaskbarItemPacket
{
    /// <summary>
    /// Gets the slot level index.
    /// </summary>
    public int SlotLevelIndex { get; private set; }

    /// <summary>
    /// Gets the taskbar slot index.
    /// </summary>
    public int SlotIndex { get; private set; }

    /// <summary>
    /// Gets the taskbar shortcut type.
    /// </summary>
    public ShortcutType Type { get; private set; }

    /// <summary>
    /// Gets the taskbar object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    /// <summary>
    /// Gets the taskbar shortcut object type.
    /// </summary>
    public ShortcutObjectType ObjectType { get; private set; }

    /// <summary>
    /// Gets the taskbar object index.
    /// </summary>
    public uint ObjectIndex { get; private set; }

    /// <summary>
    /// Gets the user id.
    /// </summary>
    public uint UserId { get; private set; }

    /// <summary>
    /// Gets the taskbar object data.
    /// </summary>
    public uint ObjectData { get; private set; }

    /// <summary>
    /// Gets the taskbar text object.
    /// </summary>
    public string Text { get; private set; }

    public AddTaskbarItemPacket(FFPacket packet)
    {
        SlotLevelIndex = packet.ReadByte();
        SlotIndex = packet.ReadByte();
        Type = (ShortcutType)packet.ReadUInt32();
        ObjectId = packet.ReadUInt32();
        ObjectType = (ShortcutObjectType)packet.ReadUInt32();
        ObjectIndex = packet.ReadUInt32();
        UserId = packet.ReadUInt32();
        ObjectData = packet.ReadUInt32();
        Text = Type == ShortcutType.Chat ? packet.ReadString() : string.Empty;
    }
}