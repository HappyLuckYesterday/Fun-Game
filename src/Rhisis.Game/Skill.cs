using Rhisis.Core.IO;
using Rhisis.Game.Entities;
using Rhisis.Game.Resources.Properties;
using Rhisis.Protocol;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rhisis.Game;

[DebuggerDisplay("{Name} Lv.{Level}")]
public class Skill
{
    private int _level;
    private long _nextSkillUsageTime;

    public int Id => Properties.Id;

    public Mover Owner { get; }

    public int? DatabaseId { get; set; }

    public int Level
    {
        get => _level;
        set => _level = Math.Clamp(value, 0, Properties.MaxLevel);
    }

    public string Name => Properties.Name;

    public SkillProperties Properties { get; }

    public SkillLevelProperties LevelData => Properties.SkillLevels[Level];

    public Skill(SkillProperties skillProperties, Mover owner, int level, int? databaseId = null)
    {
        Properties = skillProperties ?? throw new ArgumentNullException(nameof(skillProperties), "Cannot create a skill instance with undefined skill properties.");
        Owner = owner;
        Level = level;
        DatabaseId = databaseId;
    }

    public int GetCastingTime()
    {
        if (Properties.Type == SkillType.Skill)
        {
            return 1000;
        }
        else
        {
            int castingTime = (int)((LevelData.CastingTime / 1000f) * (60 / 4));

            castingTime -= castingTime * (Owner.Attributes.Get(DefineAttributes.DST_SPELL_RATE) / 100);

            return Math.Max(castingTime, 0);
        }
    }

    public void SetCoolTime(long coolTime)
    {
        if (coolTime > 0)
        {
            _nextSkillUsageTime = Time.GetElapsedTime() + coolTime;
        }
    }

    public bool IsCoolTimeElapsed() => _nextSkillUsageTime < Time.GetElapsedTime();

    public void Serialize(FFPacket packet)
    {
        packet.WriteInt32(Id);
        packet.WriteInt32(Level);
    }

    public bool Equals([AllowNull] Skill otherSkill) => Id == otherSkill?.Id && Owner.ObjectId == otherSkill?.Owner.ObjectId;

    public bool CanUse(Mover target)
    {
        return true;
        //return _skillSystem.Value.CanUseSkill(Owner as IPlayer, target, this);
    }

    public void Use(Mover target, SkillUseType skillUseType = SkillUseType.Normal)
    {
        //_skillSystem.Value.UseSkill(Owner as IPlayer, target, this, skillUseType);
    }

    public override string ToString() => Name;
}