using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ScriptEquipItemPacket
{
    /// <summary>
    /// Gets the item id.
    /// </summary>
    public uint ItemId { get; private set; }

    /// <summary>
    /// Gets the option.
    /// </summary>
    public int Option { get; private set; }

    public ScriptEquipItemPacket(FFPacket packet)
    {
        ItemId = packet.ReadUInt32();
        Option = packet.ReadInt32();
    }
}