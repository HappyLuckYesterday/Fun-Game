using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Party;

public class PartySkillUsePacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the skill id.
    /// </summary>
    public int SkillId { get; private set; }


    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        SkillId = packet.ReadInt32();
    }
}