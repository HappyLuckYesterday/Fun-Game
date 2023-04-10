using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Taskbar;

public class RemoveTaskbarItemPacket
{
    /// <summary>
    ///  Gets the slot level index from the item taskbar to be removed.
    /// </summary>
    public int SlotLevelIndex { get; private set; }

    /// <summary>
    /// Gets the slot index from the applet taskbar to be removed.
    /// </summary>
    public int SlotIndex { get; private set; }

    /// <inheritdoc />
    public RemoveTaskbarItemPacket(FFPacket packet)
    {
        SlotLevelIndex = packet.ReadByte();
        SlotIndex = packet.ReadByte();
    }
}