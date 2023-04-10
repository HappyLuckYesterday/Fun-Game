using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class AwakeningPacket
{
    /// <summary>
    /// Gets the item id.
    /// </summary>
    public int Item { get; private set; }

    public AwakeningPacket(FFPacket packet)
    {
        Item = packet.ReadInt32();
    }
}