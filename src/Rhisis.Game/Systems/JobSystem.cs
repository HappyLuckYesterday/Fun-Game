using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Protocol.Snapshots.Skills;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using System.Collections.Generic;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public class JobSystem : GameFeature, IJobSystem
    {
        private readonly IGameResources _gameResources;

        public JobSystem(IGameResources gameResources)
        {
            _gameResources = gameResources;
        }

        public void ChangeJob(IPlayer player, DefineJob.Job newJob)
        {
            if (!_gameResources.Jobs.TryGetValue(newJob, out JobData jobData))
            {
                throw new KeyNotFoundException($"Cannot find job '{newJob}'.");
            }

            IEnumerable<SkillData> jobSkills = _gameResources.GetSkillDataByJob(newJob);

            var skillTree = new List<ISkill>();

            foreach (SkillData skillData in jobSkills)
            {
                ISkill playerSkill = player.SkillTree.GetSkill(skillData.Id);

                if (playerSkill != null)
                {
                    skillTree.Add(playerSkill);
                }
                else
                {
                    skillTree.Add(new Skill(skillData, player, 0));
                }
            }

            player.SkillTree.SetSkills(skillTree);
            player.Job = jobData;

            using var snapshots = new FFSnapshot(
                new SetJobSkill(player),
                new CreateSfxObjectSnapshot(player, DefineSpecialEffects.XI_GEN_LEVEL_UP01));

            SendPacketToVisible(player, snapshots, sendToPlayer: true);
            player.UpdateCache();
        }

        public int GetJobMinLevel(DefineJob.Job job)
        {
            JobData jobData = _gameResources.Jobs.GetValueOrDefault(job);

            if (jobData == null)
            {
                return 0;
            }

            return jobData.Type switch
            {
                DefineJob.JobType.JTYPE_BASE => 1,
                DefineJob.JobType.JTYPE_EXPERT => (int)DefineJob.JobMax.MAX_JOB_LEVEL,
                DefineJob.JobType.JTYPE_PRO => (int)DefineJob.JobMax.MAX_JOB_LEVEL + (int)DefineJob.JobMax.MAX_EXP_LEVEL,
                _ => (int)DefineJob.JobMax.MAX_LEVEL
            };
        }

        public int GetJobMaxLevel(DefineJob.Job job)
        {
            JobData jobData = _gameResources.Jobs.GetValueOrDefault(job);

            if (jobData == null)
            {
                return 0;
            }

            return jobData.Type switch
            {
                DefineJob.JobType.JTYPE_BASE => (int)DefineJob.JobMax.MAX_JOB_LEVEL,
                DefineJob.JobType.JTYPE_EXPERT => (int)DefineJob.JobMax.MAX_JOB_LEVEL + (int)DefineJob.JobMax.MAX_EXP_LEVEL,
                DefineJob.JobType.JTYPE_PRO => (int)DefineJob.JobMax.MAX_LEVEL,
                _ => (int)DefineJob.JobMax.MAX_LEVEL
            };
        }
    }
}
