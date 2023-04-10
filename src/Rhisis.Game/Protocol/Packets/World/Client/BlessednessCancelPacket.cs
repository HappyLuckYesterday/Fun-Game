using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class BlessednessCancelPacket
{
    /// <summary>
    /// Gets the item id.
    /// </summary>
    public int Item { get; private set; }

    public BlessednessCancelPacket(FFPacket packet)
    {
        Item = packet.ReadInt32();
    }
}