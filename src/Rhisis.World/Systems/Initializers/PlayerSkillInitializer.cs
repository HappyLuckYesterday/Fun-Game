using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Resources;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Skills;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Initializers
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class PlayerSkillInitializer : IGameSystemLifeCycle
    {
        private readonly IRhisisDatabase _database;
        private readonly IGameResources _gameResources;
        private readonly ISkillSystem _skillSystem;

        public int Order => 1;

        public PlayerSkillInitializer(IRhisisDatabase database, IGameResources gameResources, ISkillSystem skillSystem)
        {
            _database = database;
            _gameResources = gameResources;
            _skillSystem = skillSystem;
        }

        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<Skill> jobSkills = _skillSystem.GetSkillsByJob(player.PlayerData.Job);
            IEnumerable<DbSkill> playerSkills = _database.Skills.Where(x => x.CharacterId == player.PlayerData.Id).AsNoTracking().AsEnumerable();

            player.SkillTree.Skills = (from x in jobSkills
                                       join s in playerSkills on x.SkillId equals s.SkillId into dbSkills
                                       from dbSkill in dbSkills.DefaultIfEmpty()
                                       select new Skill(x.SkillId, player.PlayerData.Id, _gameResources.Skills[x.SkillId], dbSkill?.Level ?? default, dbSkill?.Id)).ToList();
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            var skillsSet = from x in _database.Skills.Where(x => x.CharacterId == player.PlayerData.Id).AsNoTracking().ToList()
                            join s in player.SkillTree.Skills on
                             new { x.SkillId, x.CharacterId }
                             equals
                             new { s.SkillId, s.CharacterId }
                            select new { DbSkill = x, PlayerSkill = s };

            foreach (var skillToUpdate in skillsSet)
            {
                skillToUpdate.DbSkill.Level = (byte)skillToUpdate.PlayerSkill.Level;

                _database.Skills.Update(skillToUpdate.DbSkill);
            }

            foreach (Skill skill in player.SkillTree.Skills)
            {
                if (!skill.DatabaseId.HasValue && skill.Level > 0)
                {
                    var newSkill = new DbSkill
                    {
                        SkillId = skill.SkillId,
                        Level = (byte)skill.Level,
                        CharacterId = player.PlayerData.Id
                    };

                    _database.Skills.Add(newSkill);
                }
            }

            _database.SaveChanges();
        }
    }
}
