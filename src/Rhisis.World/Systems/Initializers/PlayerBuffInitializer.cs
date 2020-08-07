using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Initializers
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class PlayerBuffInitializer : IGameSystemLifeCycle
    {
        private readonly IRhisisDatabase _database;
        private readonly IAttributeSystem _attributeSystem;

        public int Order => 0;

        public PlayerBuffInitializer(IRhisisDatabase database, IAttributeSystem attributeSystem)
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

                if (!buff.HasExpired)
                {
                    player.Buffs.Add(buff);

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

            foreach (BuffSkill buffSkill in player.Buffs)
            {
                DbSkillBuff dbBuff = buffs.FirstOrDefault(x => x.SkillId == buffSkill.SkillId);

                if (dbBuff == null)
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
                else
                {
                    dbBuff.SkillLevel = buffSkill.SkillLevel;
                    dbBuff.RemainingTime = buffSkill.RemainingTime;
                    dbBuff.Attributes = buffSkill.Attributes.Select(x => new DbSkillBuffAttribute
                    {
                        AttributeId = (int)x.Key,
                        Value = x.Value
                    }).ToList();
                }
            }

            _database.SaveChanges();
        }
    }
}
