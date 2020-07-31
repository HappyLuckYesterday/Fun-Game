using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
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
        private readonly IRhisisDatabase _database;
        private readonly IAttributeSystem _attributeSystem;

        public int Order => 0;

        public BuffSystem(IRhisisDatabase database, IAttributeSystem attributeSystem)
        {
            _database = database;
            _attributeSystem = attributeSystem;
        }

        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<DbSkillBuff> skillBuffs = _database.SkillBuffs
                .Include(x => x.Attributes)
                .Where(x => x.CharacterId == player.PlayerData.Id)
                .AsNoTracking()
                .AsEnumerable();

            player.Buffs.Clear();

            foreach (DbSkillBuff dbSkillBuff in skillBuffs)
            {
                IDictionary<DefineAttributes, int> buffAttributes = dbSkillBuff.Attributes.ToDictionary(x => (DefineAttributes)x.AttributeId, x => x.Value);

                var buff = new BuffSkill(dbSkillBuff.SkillId, dbSkillBuff.SkillLevel, dbSkillBuff.RemainingTime, buffAttributes, dbSkillBuff.Id);

                if (AddBuff(player, buff))
                {
                    foreach (KeyValuePair<DefineAttributes, int> buffAttribute in buffAttributes)
                    {
                        _attributeSystem.SetAttribute(player, buffAttribute.Key, buffAttribute.Value, sendToEntity: false);
                    }
                }
            }
        }

        public void Save(IPlayerEntity player)
        {
            var buffs = _database.SkillBuffs
                .Include(x => x.Attributes)
                .Where(x => x.CharacterId == player.PlayerData.Id)
                .ToList();

            foreach (DbSkillBuff buffSkill in buffs)
            {
                buffSkill.RemainingTime = 0;
                buffSkill.Attributes.Clear();
            }

            var buffSkillSet = from x in buffs
                               join b in player.Buffs.OfType<BuffSkill>() on x.SkillId equals b.SkillId
                               select new { DbBuff = x, Buff = b };

            foreach (var buffSet in buffSkillSet)
            {
                buffSet.DbBuff.SkillLevel = buffSet.Buff.SkillLevel;
                buffSet.DbBuff.RemainingTime = buffSet.Buff.RemainingTime;
                buffSet.DbBuff.Attributes = buffSet.Buff.Attributes.Select(x => new DbSkillBuffAttribute
                {
                    AttributeId = (int)x.Key,
                    Value = x.Value
                }).ToList();
            }

            foreach (BuffSkill buffSkill in player.Buffs)
            {
                if (!buffSkill.DatabaseId.HasValue)
                {
                    _database.SkillBuffs.Add(new DbSkillBuff
                    {
                        SkillId = buffSkill.SkillId,
                        SkillLevel = buffSkill.SkillLevel,
                        RemainingTime = buffSkill.RemainingTime,
                        Attributes = buffSkill.Attributes.Select(x => new DbSkillBuffAttribute
                        {
                            AttributeId = (int)x.Key,
                            Value = x.Value
                        }).ToList(),
                        CharacterId = player.PlayerData.Id,
                    });
                }
            }

            _database.SaveChanges();
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
