using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ScriptRemoveQuestPacket
{
    /// <summary>
    /// Gets the gold.
    /// </summary>
    public int QuestId { get; private set; }

    public ScriptRemoveQuestPacket(FFPacket packet)
    {
        QuestId = packet.ReadInt32();
    }
}