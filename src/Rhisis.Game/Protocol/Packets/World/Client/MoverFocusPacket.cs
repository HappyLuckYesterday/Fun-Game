using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class MoverFocusPacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    public MoverFocusPacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
    }
}