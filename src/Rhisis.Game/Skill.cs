using Rhisis.Core.IO;
using Rhisis.Game.Abstractions;
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
        private int _level;
        private long _nextSkillUsageTime;

        public int Id => Data.Id;

        public int CharacterId { get; set; }

        public int? DatabaseId { get; set; }

        public int Level
        {
            get => _level;
            set => _level = Math.Clamp(value, 0, Data.MaxLevel);
        }

        public string Name => Data.Name;

        public SkillData Data { get; }

        public SkillLevelData LevelData => Data.SkillLevels[Level];

        public Skill(SkillData skillData, int characterId, int? databaseId = null)
        {
            Data = skillData;
            CharacterId = characterId;
            DatabaseId = databaseId;
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

        public bool Equals([AllowNull] ISkill otherSkill) => Id == otherSkill?.Id && CharacterId == otherSkill?.CharacterId;
    }
