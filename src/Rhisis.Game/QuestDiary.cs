using Rhisis.Protocol;
using System.Collections.Generic;
using System;
using System.Linq;
using Rhisis.Game.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game;

public sealed class QuestDiary
{
    private readonly Player _player;
    private readonly List<Quest> _quests = new();

    public IEnumerable<Quest> ActiveQuests => _quests.Where(x => !x.IsFinished);

    public IEnumerable<Quest> CheckedQuests => ActiveQuests.Where(x => x.IsChecked);

    public IEnumerable<Quest> CompletedQuests => _quests.Where(x => x.IsFinished);

    public QuestDiary(Player owner)
    {
        _player = owner;
    }

    public void Add(Quest quest)
    {
        if (quest is null)
        {
            throw new ArgumentNullException(nameof(quest), "Cannot add an undefined quest.");
        }

        if (_quests.Contains(quest))
        {
            throw new InvalidOperationException($"Quest '{quest.Id}' for player with id '{quest.CharacterId}' already exists.");
        }

        _quests.Add(quest);
    }

    public Quest GetQuest(int questId) => _quests.FirstOrDefault(x => x.Id == questId);

    public bool HasQuest(int questId) => _quests.Any(x => x.Id == questId);

    public void Remove(Quest quest)
    {
        quest.IsDeleted = true;
    }

    public void Update(QuestActionType actionType, params object[] values)
    {
    }

    public void Serialize(FFPacket packet)
    {
        packet.WriteByte((byte)ActiveQuests.Count());
        foreach (Quest quest in ActiveQuests)
        {
            quest.Serialize(packet);
        }

        packet.WriteByte((byte)CompletedQuests.Count());
        foreach (Quest quest in CompletedQuests)
        {
            packet.WriteInt16((short)quest.Id);
        }

        packet.WriteByte((byte)CheckedQuests.Count());
        foreach (Quest quest in CheckedQuests)
        {
            packet.WriteInt16((short)quest.Id);
        }
    }
}
