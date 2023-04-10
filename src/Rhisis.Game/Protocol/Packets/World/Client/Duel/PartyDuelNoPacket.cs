using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Duel;

public class PartyDuelNoPacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    public PartyDuelNoPacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
    }
}