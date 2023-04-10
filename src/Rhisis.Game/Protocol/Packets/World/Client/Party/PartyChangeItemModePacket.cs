using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyChangeItemModePacket
{

    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the item mode.
    /// </summary>
    public int ItemMode { get; private set; }

    public PartyChangeItemModePacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        ItemMode = packet.ReadInt32();
    }
}