using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Buff
{
    [Injectable(ServiceLifetime.Transient)]
    public class BuffSystem : IBuffSystem
    {
        private readonly IRhisisDatabase _database;

        public int Order => 0;

        public BuffSystem(IRhisisDatabase database)
        {
            _database = database;
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

                player.Buffs.Add(buff);
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
    }
}
