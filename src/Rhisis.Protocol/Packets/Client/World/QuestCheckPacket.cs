using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class QuestCheckPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the quest id.
    /// </summary>
    public int QuestId { get; private set; }

    /// <summary>
    /// Gets the quest checked state.
    /// </summary>
    public bool Checked { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        QuestId = packet.ReadInt32();
        Checked = packet.ReadBoolean();
    }
}