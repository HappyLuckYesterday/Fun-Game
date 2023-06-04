using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Resources.Properties;
using Rhisis.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game;

public class BuffSkill : Buff
{
    public override BuffType Type => BuffType.Skill;

    /// <summary>
    /// Gets the buff database id.
    /// </summary>
    public int? DatabaseId { get; }

    /// <summary>
    /// Gets the buff skill id.
    /// </summary>
    public int SkillId => SkillProperties.Id;

    /// <summary>
    /// Gets the skill name.
    /// </summary>
    public string SkillName => SkillProperties.Name;

    /// <summary>
    /// Gets the skill level.
    /// </summary>
    public int SkillLevel { get; }

    /// <summary>
    /// Gets the skill properties.
    /// </summary>
    public SkillProperties SkillProperties { get; }

    /// <summary>
    /// Gets the skill level properties.
    /// </summary>
    public SkillLevelProperties SkillLevelProperties => SkillProperties.SkillLevels[SkillLevel];

    public BuffSkill(Mover owner, IDictionary<DefineAttributes, int> attributes, SkillProperties skillProperties, int skillLevel, int? databaseId = null)
        : base(owner, attributes)
    {
        DatabaseId = databaseId;
        SkillProperties = skillProperties;
    }

    public override bool Equals(object obj)
    {
        if (obj is BuffSkill buffSkill)
        {
            return SkillId == buffSkill.SkillId;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode() => (int)Id;

    public override void Serialize(FFPacket packet)
    {
        packet.WriteInt16((short)Type);
        packet.WriteInt16((short)SkillId);
        packet.WriteInt32(SkillLevel);
        packet.WriteInt32(RemainingTime);
    }
}