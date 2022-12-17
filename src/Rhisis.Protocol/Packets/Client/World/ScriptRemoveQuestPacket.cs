using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class ScriptRemoveQuestPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the gold.
    /// </summary>
    public int QuestId { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        QuestId = packet.ReadInt32();
    }
}