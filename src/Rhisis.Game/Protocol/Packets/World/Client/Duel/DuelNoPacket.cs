using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Duel;

public class DuelNoPacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    public DuelNoPacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
    }
}