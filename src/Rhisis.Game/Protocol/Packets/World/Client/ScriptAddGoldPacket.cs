using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ScriptAddGoldPacket
{
    /// <summary>
    /// Gets the gold.
    /// </summary>
    public int Gold { get; private set; }

    public ScriptAddGoldPacket(FFPacket packet)
    {
        Gold = packet.ReadInt32();
    }
}