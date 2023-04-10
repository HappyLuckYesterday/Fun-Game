using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyMemberRequestPacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the member id.
    /// </summary>
    public uint MemberId { get; private set; }

    /// <summary>
    /// Gets if it's a troup.
    /// </summary>
    public bool Troup { get; private set; }

    public PartyMemberRequestPacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        MemberId = packet.ReadUInt32();
        Troup = packet.ReadInt32() == 1;
    }
}