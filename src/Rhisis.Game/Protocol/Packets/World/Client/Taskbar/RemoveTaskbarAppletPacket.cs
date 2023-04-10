using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Taskbar;

public class RemoveTaskbarAppletPacket
{
    /// <summary>
    /// Gets the slot index from the applet taskbar to be removed.
    /// </summary>
    public int SlotIndex { get; private set; }

    public RemoveTaskbarAppletPacket(FFPacket packet)
    {
        SlotIndex = packet.ReadByte();
    }
}