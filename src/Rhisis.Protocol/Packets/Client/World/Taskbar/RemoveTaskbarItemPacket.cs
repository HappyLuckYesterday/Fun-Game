using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Taskbar;

public class RemoveTaskbarItemPacket : RemoveTaskbarAppletPacket
{
    /// <summary>
    ///  Gets the slot level index from the item taskbar to be removed.
    /// </summary>
    public int SlotLevelIndex { get; private set; }

    /// <inheritdoc />
    public override void Deserialize(IFFPacket packet)
    {
        SlotLevelIndex = packet.ReadByte();
        base.Deserialize(packet);
    }
}