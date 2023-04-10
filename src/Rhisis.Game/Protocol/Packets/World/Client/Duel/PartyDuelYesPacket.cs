using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Duel;

public class PartyDuelYesPacket
{
    /// <summary>
    /// Gets the source player id.
    /// </summary>
    public uint SourcePlayerId { get; private set; }

    /// <summary>
    /// Gets the destination player id.
    /// </summary>
    public uint DestinationPlayerId { get; private set; }

    public PartyDuelYesPacket(FFPacket packet)
    {
        SourcePlayerId = packet.ReadUInt32();
        DestinationPlayerId = packet.ReadUInt32();
    }
}