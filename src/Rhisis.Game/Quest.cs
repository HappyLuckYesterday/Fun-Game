using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using Rhisis.Game.Resources.Properties.Quests;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public sealed class Quest : IPacketSerializer
{
    private readonly Player _owner;

    /// <summary>
    /// Gest the quest properties.
    /// </summary>
    public QuestProperties Properties { get; }

    /// <summary>
    /// Gets the quest id.
    /// </summary>
    public int Id => Properties.Id;

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
    /// Gets or sets the quest start time.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the quest end time.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Gets a dictionary of the killed monsters. Key is the monster id; Value is the killed amount.
    /// </summary>
    public IDictionary<int, short> Monsters { get; set; }

    public Quest(QuestProperties properties, Player owner)
    {
        Properties = properties ?? throw new ArgumentNullException(nameof(properties), "Cannot create a quest with undefined quest properties.");
        Monsters = Properties.QuestEndCondition.Monsters?.ToDictionary(x => GameResources.Current.GetDefinedValue(x.Id), _ => (short)0);
        _owner = owner;
    }

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

    /// <summary>
    /// Checks if the quest can be finished.
    /// </summary>
    /// <returns>True if the quest can be finished; false otherwise.</returns>
    public bool CanFinish()
    { 
        if (Properties.QuestEndCondition.Items != null && Properties.QuestEndCondition.Items.Any())
        {
            foreach (QuestItemProperties questItem in Properties.QuestEndCondition.Items)
            {
                if (questItem.Sex == GenderType.Any || questItem.Sex == _owner.Appearence.Gender)
                {
                    ItemProperties itemProperties = GameResources.Current.Items.Get(questItem.Id);

                    if (itemProperties is null)
                    {
                        continue;
                    }

                    Item inventoryItem = _owner.Inventory.FindItem(x => x.Id == itemProperties.Id);

                    if (inventoryItem is null || inventoryItem.Quantity < questItem.Quantity)
                    {
                        return false;
                    }
                }
            }
        }

        if (Properties.QuestEndCondition.Monsters != null && Properties.QuestEndCondition.Monsters.Any())
        {
            foreach (QuestMonsterProperties questMonster in Properties.QuestEndCondition.Monsters)
            {
                MoverProperties monsterProperties = GameResources.Current.Movers.Get(questMonster.Id);

                if (monsterProperties is null)
                {
                    continue;
                }

                if (Monsters.TryGetValue(monsterProperties.Id, out short killedQuantity))
                {
                    if (killedQuantity < questMonster.Amount)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
