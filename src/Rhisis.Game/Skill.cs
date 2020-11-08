using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.IO;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Sylver.Network.Data;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rhisis.Game
{
    [DebuggerDisplay("{Name} Lv.{Level}")]
    public class Skill : ISkill
    {
        private readonly Lazy<ISkillSystem> _skillSystem;
        private int _level;
        private long _nextSkillUsageTime;

        public int Id => Data.Id;

        public IMover Owner { get; }

        public int? DatabaseId { get; set; }

        public int Level
        {
            get => _level;
            set => _level = Math.Clamp(value, 0, Data.MaxLevel);
        }

        public string Name => Data.Name;

        public SkillData Data { get; }

        public SkillLevelData LevelData => Data.SkillLevels[Level];

        public Skill(SkillData skillData, IMover owner, int level, int? databaseId = null)
        {
            Data = skillData;
            Owner = owner;
            Level = level;
            DatabaseId = databaseId;
            _skillSystem = new Lazy<ISkillSystem>(() => Owner.Systems.GetService<ISkillSystem>());
        }

        public int GetCastingTime()
        {
            if (Data.Type == SkillType.Skill)
            {
                return 1000;
            }
            else
            {
                int castingTime = (int)((LevelData.CastingTime / 1000f) * (60 / 4));

                castingTime -= castingTime * (Owner.Attributes.Get(DefineAttributes.SPELL_RATE) / 100);

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

        public void Serialize(INetPacketStream packet)
        {
            packet.Write(Id);
            packet.Write(Level);
        }

        public bool Equals([AllowNull] ISkill otherSkill) => Id == otherSkill?.Id && Owner.Id == otherSkill?.Owner.Id;

        public bool CanUse(IMover target)
        {
            return _skillSystem.Value.CanUseSkill(Owner as IPlayer, target, this);
        }

        public void Use(IMover target, SkillUseType skillUseType = SkillUseType.Normal)
        {
            _skillSystem.Value.UseSkill(Owner as IPlayer, target, this, skillUseType);
        }
    }
}