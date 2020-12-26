using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rhisis.Game
{
    [DebuggerDisplay("Buff '{SkillName}' Lv.{SkillLevel}")]
    public class BuffSkill : Buff, IBuffSkill
    {
        public override BuffType Type => BuffType.Skill;

        public int? DatabaseId { get; }

        public int SkillId => SkillData.Id;

        public string SkillName => SkillData.Name;

        public int SkillLevel { get; }

        public SkillData SkillData { get; }

        public SkillLevelData SkillLevelData => SkillData.SkillLevels[SkillLevel];

        public BuffSkill(IMover owner, IDictionary<DefineAttributes, int> attributes, SkillData skillData, int skillLevel, int? databaseId = null) 
            : base(owner, attributes)
        {
            SkillData = skillData;
            SkillLevel = skillLevel;
            DatabaseId = databaseId;
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

        public override void Serialize(INetPacketStream packet)
        {
            packet.WriteInt16((short)Type);
            packet.WriteInt16((short)SkillId);
            packet.WriteInt32(SkillLevel);
            packet.WriteInt32(RemainingTime);
        }
    }
}