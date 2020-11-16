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
    [Injectable]
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
            IEnumerable<DbSkill> playerSkills = _database.Skills
                .Where(x => x.CharacterId == player.CharacterId)
                .AsNoTracking()
                .AsEnumerable();
            IEnumerable<SkillData> jobSkills = _gameResources.GetSkillDataByJob(player.Job.Id);

            IEnumerable<ISkill> skills = (from x in jobSkills
                                          join s in playerSkills on x.Id equals s.SkillId into dbSkills
                                          from dbSkill in dbSkills.DefaultIfEmpty()
                                          select new Skill(_gameResources.Skills[x.Id], player, dbSkill?.Level ?? 0, dbSkill?.Id))
                                          .ToList();

            player.SkillTree.SetSkills(skills);
        }

        /// <inheritdoc />
        public void Save(IPlayer player)
        {
            IEnumerable<DbSkill> dbSkills = _database.Skills
                .Where(x => x.CharacterId == player.CharacterId)
                .AsNoTracking()
                .AsEnumerable();
            var skillsSet = from x in dbSkills
                            join s in player.SkillTree on
                             new { x.SkillId, x.CharacterId }
                             equals
                             new { SkillId = s.Id, CharacterId = (s.Owner as IPlayer).CharacterId }
                            select new { DbSkill = x, PlayerSkill = s };

            var updatedSkillIds = new HashSet<int>();
            foreach (var skillToUpdate in skillsSet)
            {
                skillToUpdate.DbSkill.Level = (byte)skillToUpdate.PlayerSkill.Level;
                _database.Skills.Update(skillToUpdate.DbSkill);

                updatedSkillIds.Add(skillToUpdate.DbSkill.SkillId);

                // Note: skills of level 0 are not stored in database, but skills that were once leveled by a player 
                //      but are now level 0 again stay in the database with level 0
            }

            foreach (ISkill skill in player.SkillTree)
            {
                if (!updatedSkillIds.Contains(skill.Id)
                    && !skill.DatabaseId.HasValue && skill.Level > 0)
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
