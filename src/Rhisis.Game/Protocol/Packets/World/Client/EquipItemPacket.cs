using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class EquipItemPacket
{
    /// <summary>
    /// Gets the item index in the inventory.
    /// </summary>
    public int ItemIndex { get; private set; }

    /// <summary>
    /// Gets the equip item destination part.
    /// </summary>
    public ItemPartType Part { get; private set; }

    public EquipItemPacket(FFPacket packet)
    {
        ItemIndex = packet.ReadInt32();
        Part = (ItemPartType)packet.ReadInt32();
    }
}
