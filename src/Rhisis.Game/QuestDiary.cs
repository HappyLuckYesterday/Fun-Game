using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public sealed class QuestDiary
{
    private readonly Player _player;
    private readonly List<Quest> _quests = new();

    /// <summary>
    /// Gets the active quests.
    /// </summary>
    public IEnumerable<Quest> ActiveQuests => _quests.Where(x => !x.IsFinished);

    /// <summary>
    /// Gets the checked quests.
    /// </summary>
    public IEnumerable<Quest> CheckedQuests => ActiveQuests.Where(x => x.IsChecked);

    /// <summary>
    /// Gets the completed quests.
    /// </summary>
    public IEnumerable<Quest> CompletedQuests => _quests.Where(x => x.IsFinished);

    public QuestDiary(Player owner)
    {
        _player = owner;
    }

    /// <summary>
    /// Adds a quest to the diary.
    /// </summary>
    /// <param name="quest">Quest</param>
    /// <exception cref="ArgumentNullException">Thrown when the given quest is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the quest already exists in the diary.</exception>
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

    /// <summary>
    /// Gets the quest by it's id.
    /// </summary>
    /// <param name="questId">Quest id to look for.</param>
    /// <returns>The quest if found; null otherwise.</returns>
    public Quest GetQuest(int questId) => _quests.FirstOrDefault(x => x.Id == questId);

    /// <summary>
    /// Checks if the diary contains the quest identified by the given id.
    /// </summary>
    /// <param name="questId">Quest id.</param>
    /// <returns>True if the the diary contains the quest; false otherwise.</returns>
    public bool HasQuest(int questId) => _quests.Any(x => x.Id == questId);

    /// <summary>
    /// Removes the given quest from the diary.
    /// </summary>
    /// <param name="quest">Quest to remove.</param>
    public void Remove(Quest quest)
    {
        quest.IsDeleted = true;
    }

    /// <summary>
    /// Updates the diary with the given action;
    /// </summary>
    /// <param name="actionType">Quest action type.</param>
    /// <param name="values">Quest action values.</param>
    public void Update(QuestActionType actionType, params object[] values)
    {
    }

    /// <summary>
    /// Serializes the current diary into the given packet.
    /// </summary>
    /// <param name="packet">Packet.</param>
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
