using Rhisis.Game.Abstractions;
using Rhisis.Game.Common.Resources.Quests;
using Sylver.Network.Data;
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

        public void Serialize(INetPacketStream packet)
        {
            packet.Write<short>((short)State); // state
            packet.Write<short>(0); // time limit
            packet.Write((short)Id);

            packet.Write<short>(Monsters?.ElementAtOrDefault(0).Value ?? 0); // monster 1 killed
            packet.Write<short>(Monsters?.ElementAtOrDefault(1).Value ?? 0); // monster 2 killed
            packet.Write<byte>(Convert.ToByte(IsPatrolDone)); // patrol done
            packet.Write<byte>(0); // dialog done
        }
    }
}
