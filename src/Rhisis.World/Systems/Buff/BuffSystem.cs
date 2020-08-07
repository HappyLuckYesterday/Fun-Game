using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Buff
{
    [Injectable(ServiceLifetime.Transient)]
    public class BuffSystem : IBuffSystem
    {
        private readonly IAttributeSystem _attributeSystem;

        public int Order => 0;

        public BuffSystem(IAttributeSystem attributeSystem)
        {
            _attributeSystem = attributeSystem;
        }

        public bool AddBuff(ILivingEntity entity, Game.Structures.Buff buff)
        {
            if (buff.HasExpired)
            {
                return false;
            }

            if (OverwriteBuffSkill(entity, buff as BuffSkill))
            {
                return true;
            }

            entity.Buffs.Add(buff);

            return true;
        }

        public bool RemoveBuff(ILivingEntity entity, Game.Structures.Buff buff)
        {
            entity.Buffs.Remove(buff);

            foreach (KeyValuePair<DefineAttributes, int> attribute in buff.Attributes)
            {
                _attributeSystem.ResetAttribute(entity, attribute.Key, attribute.Value);
            }

            return true;
        }

        public void UpdateBuffTimers(ILivingEntity entity)
        {
            if (entity.Buffs.Any())
            {
                IEnumerable<Game.Structures.Buff> buffs = entity.Buffs.ToList();

                foreach (Game.Structures.Buff buff in buffs)
                {
                    buff.DecreaseTime();

                    if (buff.HasExpired)
                    {
                        OnBuffHasExpired(buff);
                    }
                }

                IEnumerable<Game.Structures.Buff> expiredBuffs = buffs.Where(x => x.HasExpired);

                if (expiredBuffs.Any())
                {
                    foreach (var buff in expiredBuffs)
                    {
                        RemoveBuff(entity, buff);
                    }
                }
            }
        }

        private void OnBuffHasExpired(Game.Structures.Buff buff)
        {
            // TODO: Implement custom logic when a buff has expired
        }

        private bool OverwriteBuffSkill(ILivingEntity entity, BuffSkill buffSkill)
        {
            if (buffSkill == null)
            {
                return false;
            }

            BuffSkill existingBuff = entity.Buffs.OfType<BuffSkill>().FirstOrDefault(x => x.SkillId == buffSkill.SkillId);

            if (existingBuff != null)
            {
                if (existingBuff.SkillLevel == buffSkill.SkillLevel)
                {
                    existingBuff.RemainingTime = buffSkill.RemainingTime;
                    return true;
                }
                else if (existingBuff.SkillLevel > buffSkill.SkillLevel)
                {
                    return true;
                }

                RemoveBuff(entity, existingBuff);
            }

            return false;
        }
    }
}
