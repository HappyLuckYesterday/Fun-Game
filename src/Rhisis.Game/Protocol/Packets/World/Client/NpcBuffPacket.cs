using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class NpcBuffPacket
{
    /// <summary>
    /// Gets the buffing NPC key.
    /// </summary>
    public string NpcKey { get; private set; }

    public NpcBuffPacket(FFPacket packet)
    {
        NpcKey = packet.ReadString();
    }
}