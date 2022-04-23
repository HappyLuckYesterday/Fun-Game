using Rhisis.Abstractions;
using Rhisis.Abstractions.Protocol;
using Rhisis.Game.Common.Resources.Quests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game
{
    public class Quest : IQuest
    {
        public int Id => Script.Id;

        public int? DatabaseQuestId { get; }

        public int CharacterId { get; }

        public bool IsChecked { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsFinished { get; set; }

        public bool IsPatrolDone { get; set; }

        public QuestStateType State { get; set; }

        public DateTime StartTime { get; set; }

        public IDictionary<int, short> Monsters { get; set; }

        public IQuestScript Script { get; }

        public Quest(IQuestScript script, int characterId)
        {
            Script = script;
            CharacterId = characterId;
            Monsters = new Dictionary<int, short>();
            StartTime = DateTime.UtcNow;
        }

        public Quest(IQuestScript script, int characterId, int questDatabaseId)
            : this(script, characterId)
        {
            DatabaseQuestId = questDatabaseId;
        }

        public void Serialize(IFFPacket packet)
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
}
