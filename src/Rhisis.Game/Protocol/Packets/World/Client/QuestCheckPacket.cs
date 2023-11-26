using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class QuestCheckPacket
{
    /// <summary>
    /// Gets the quest id.
    /// </summary>
    public int QuestId { get; private set; }

    /// <summary>
    /// Gets the quest checked state.
    /// </summary>
    public bool Checked { get; private set; }

    public QuestCheckPacket(FFPacket packet)
    {
        QuestId = packet.ReadInt32();
        Checked = packet.ReadBoolean();
    }
}
