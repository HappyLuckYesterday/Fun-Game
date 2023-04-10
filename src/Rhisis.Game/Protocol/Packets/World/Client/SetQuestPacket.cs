using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class SetQuestPacket
{
    /// <summary>
    /// Gets the quest id.
    /// </summary>
    public int QuestId { get; private set; }

    /// <summary>
    /// Gets the state.
    /// </summary>
    public int State { get; private set; }

    public SetQuestPacket(FFPacket packet)
    {
        QuestId = packet.ReadInt32();
        State = packet.ReadInt32();
    }
}