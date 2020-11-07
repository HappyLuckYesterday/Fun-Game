using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common.Resources;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Initializers
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class PlayerSkillInitializer : IPlayerInitializer
    {
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;

        public int Order => 1;

        public PlayerSkillInitializer(IRhisisDatabase database, IGameResources gameResources)
        {
            _database = database;
            _gameResources = gameResources;
        }

        public void Load(IPlayer player)
        {
            IEnumerable<SkillData> jobSkills = _gameResources.GetSkillDataByJob(player.Job.Id);
            IEnumerable<DbSkill> playerSkills = _database.Skills.Where(x => x.CharacterId == player.CharacterId).AsNoTracking().AsEnumerable();

            IEnumerable<ISkill> skills = (from x in jobSkills
                                          join s in playerSkills on x.Id equals s.SkillId into dbSkills
                                          from dbSkill in dbSkills.DefaultIfEmpty()
                                          select new Skill(_gameResources.Skills[x.Id], player.CharacterId, dbSkill?.Id)).ToList();

            player.SkillTree.SetSkills(skills);
        }

        /// <inheritdoc />
        public void Save(IPlayer player)
        {
            var skillsSet = from x in _database.Skills.Where(x => x.CharacterId == player.CharacterId).AsNoTracking().ToList()
                            join s in player.SkillTree on
                             new { x.SkillId, x.CharacterId }
                             equals
                             new { SkillId = s.Id, s.CharacterId }
                            select new { DbSkill = x, PlayerSkill = s };

            foreach (var skillToUpdate in skillsSet)
            {
                skillToUpdate.DbSkill.Level = (byte)skillToUpdate.PlayerSkill.Level;

                _database.Skills.Update(skillToUpdate.DbSkill);
            }

            foreach (ISkill skill in player.SkillTree)
            {
                if (!skill.DatabaseId.HasValue && skill.Level > 0)
                {
                    var newSkill = new DbSkill
                    {
                        SkillId = skill.Id,
                        Level = (byte)skill.Level,
                        CharacterId = player.CharacterId
                    };

                    _database.Skills.Add(newSkill);
                }
            }

            _database.SaveChanges();
        }
    }
}
