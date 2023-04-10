using Rhisis.Protocol;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Rhisis.Game;

public sealed class Quest
{
    public int Id { get; }

    public int? DatabaseQuestId { get; }

    public int CharacterId { get; }

    public bool IsChecked { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsFinished { get; set; }

    public bool IsPatrolDone { get; set; }

    public QuestState State { get; set; }

    public DateTime StartTime { get; set; }

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
