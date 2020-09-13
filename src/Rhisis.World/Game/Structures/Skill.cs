using Rhisis.Core.IO;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common.Resources;
using Sylver.Network.Data;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rhisis.World.Game.Structures
{
    [DebuggerDisplay("{Name} Lv. {Level}")]
    public class Skill : IEquatable<Skill>, IPacketSerializer
    {
        private long _nextSkillUsageTime;

        /// <summary>
        /// Gets the skill id.
        /// </summary>
        public int SkillId { get; }

        /// <summary>
        /// Gets the character id owner of this skill.
        /// </summary>
        public int CharacterId { get; }

        /// <summary>
        /// Gets the skill database id.
        /// </summary>
        public int? DatabaseId { get; }

        /// <summary>
        /// Gets the skill name.
        /// </summary>
        public string Name => Data?.Name ?? "[undefined]";

        /// <summary>
        /// Gets the skill data.
        /// </summary>
        public SkillData Data { get; }

        /// <summary>
        /// Gets the skill level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets the skill level information.
        /// </summary>
        public SkillLevelData LevelData => Data.SkillLevels[Level];

        /// <summary>
        /// Creates a new <see cref="Skill"/> instance.
        /// </summary>
        /// <param name="skillId">Skill id.</param>
        /// <param name="characterId">Character Id.</param>
        /// <param name="skillData">Skill data.</param>
        /// <param name="level">Skill level.</param>
        /// <param name="databaseId">Database id.</param>
        public Skill(int skillId, int characterId, SkillData skillData, int level = default, int? databaseId = default)
        {
            SkillId = skillId;
            CharacterId = characterId;
            Data = skillData;
            DatabaseId = databaseId;
            Level = level;
        }

        /// <summary>
        /// Sets the skill cool time before it can be used again.
        /// </summary>
        /// <param name="coolTime">Cool time.</param>
        public void SetCoolTime(long coolTime)
        {
            if (coolTime > 0)
            {
                _nextSkillUsageTime = Time.GetElapsedTime() + coolTime;
            }
        }

        /// <summary>
        /// Check if the current skill cool time is over.
        /// </summary>
        /// <returns>True if the cooltime is over; false otherwise.</returns>
        public bool IsCoolTimeElapsed() => _nextSkillUsageTime < Time.GetElapsedTime();

        /// <summary>
        /// Compares two <see cref="Skill"/> instances.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals([AllowNull] Skill other) => SkillId == other?.SkillId && CharacterId == other?.CharacterId;

        /// <inheritdoc />
        public void Serialize(INetPacketStream packet)
        {
            packet.Write(SkillId);
            packet.Write(Level);
        }

        public override string ToString() => Name;
    }
}