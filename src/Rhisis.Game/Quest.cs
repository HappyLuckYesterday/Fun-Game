using Rhisis.Game.Common;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public sealed class Quest : IPacketSerializer
{
    /// <summary>
    /// Gets the quest id.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the quest database id.
    /// </summary>
    public int? DatabaseQuestId { get; }

    /// <summary>
    /// Gets the quest character id.
    /// </summary>
    public int CharacterId { get; }

    /// <summary>
    /// Gets or sets a boolean value that indiciates the quest checked in state in the player's interface.
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the quest has been deleted or not.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the quest is finished or not.
    /// </summary>
    public bool IsFinished { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the patrol has been done for the current quest.
    /// </summary>
    public bool IsPatrolDone { get; set; }

    /// <summary>
    /// Gets or sets the quest state.
    /// </summary>
    public QuestState State { get; set; }

    /// <summary>
    /// Gets the quest start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets a dictionary of the killed monsters. Key is the monster id; Value is the killed amount.
    /// </summary>
    public IDictionary<int, short> Monsters { get; set; }

    public void Serialize(FFPacket packet)
    {
        packet.WriteInt16((short)State); // state
        packet.WriteInt16(0); // time limit
        packet.WriteInt16((short)Id);

        packet.WriteInt16(Monsters?.ElementAtOrDefault(0).Value ?? 0); // monster 1 killed
        packet.WriteInt16(Monsters?.ElementAtOrDefault(1).Value ?? 0); // monster 2 killed
        packet.WriteByte(Convert.ToByte(IsPatrolDone)); // patrol done
        packet.WriteByte(0); // dialog done
    }
}
