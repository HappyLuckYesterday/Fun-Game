using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems.Initializers
{
    [Injectable]
    public sealed class PlayerBuffInitializer : IPlayerInitializer
    {
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;
        private readonly ILogger<PlayerBuffInitializer> _logger;

        public int Order => 0;

        public PlayerBuffInitializer(IRhisisDatabase database, IGameResources gameResources, ILogger<PlayerBuffInitializer> logger)
        {
            _database = database;
            _gameResources = gameResources;
            _logger = logger;
        }

        public void Load(IPlayer player)
        {
            IEnumerable<DbSkillBuff> skillBuffs = _database.SkillBuffs
                .Include(x => x.Attributes)
                .Where(x => x.CharacterId == player.CharacterId)
                .AsNoTracking()
                .AsEnumerable();

            player.Buffs.RemoveAll();

            foreach (DbSkillBuff dbSkillBuff in skillBuffs)
            {
                IDictionary<DefineAttributes, int> buffAttributes = dbSkillBuff.Attributes.ToDictionary(x => (DefineAttributes)x.AttributeId, x => x.Value);

                SkillData skillData = _gameResources.Skills.GetValueOrDefault(dbSkillBuff.SkillId);

                if (skillData == null)
                {
                    _logger.LogWarning($"Failed to load buff skill with id: '{dbSkillBuff.SkillId}' for player {player}.");
                    continue;
                }

                var buff = new BuffSkill(player, buffAttributes, skillData, dbSkillBuff.SkillLevel, dbSkillBuff.Id)
                {
                    RemainingTime = dbSkillBuff.RemainingTime
                };

                if (!buff.HasExpired)
                {
                    player.Buffs.Add(buff);

                    foreach (KeyValuePair<DefineAttributes, int> buffAttribute in buffAttributes)
                    {
                        player.Attributes.Increase(buffAttribute.Key, buffAttribute.Value, sendToEntity: false);
                    }
                }
            }
        }

        public void Save(IPlayer player)
        {
            var buffs = _database.SkillBuffs
                .Include(x => x.Attributes)
                .Where(x => x.CharacterId == player.CharacterId)
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
                        CharacterId = player.CharacterId,
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
