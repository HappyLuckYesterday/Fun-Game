using Rhisis.Core.Extensions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Network.Snapshots;
using System;
using System.Linq;

namespace Rhisis.Game.Features.Chat.Commands
{
    // /level [job] [level] (legend)
    [ChatCommand("/level", AuthorityType.GameMaster)]
    [ChatCommand("/lv", AuthorityType.GameMaster)]
    public class LevelCommand : IChatCommand
    {
        private readonly IGameResources _gameResources;
        private readonly IJobSystem _jobSystem;

        public LevelCommand(IGameResources gameResources, IJobSystem jobSystem)
        {
            _gameResources = gameResources;
            _jobSystem = jobSystem;
        }
        
        public void Execute(IPlayer player, object[] parameters)
        {
            if (parameters.Length < 2)
            {
                using var snoop = new SnoopSnapshot("Missing arguments. /level [job] [level] (legend)");
                player.Send(snoop);

                throw new InvalidOperationException($"Failed to execute command /level. Missing arguments.");
            }

            string jobParameter = parameters[0].ToString();
            string levelParameter = parameters[1].ToString();
            string legendParameter = parameters.ElementAtOrDefault(2)?.ToString();

            if (!TryParseJob(jobParameter, out JobData job))
            {
                throw new InvalidOperationException($"Failed to parse job: '{jobParameter}'.");
            }

            int level = Math.Clamp(GetLevel(levelParameter), _jobSystem.GetJobMinLevel(job.Id), _jobSystem.GetJobMaxLevel(job.Id));

            player.Level = level;
            player.DeathLevel = 0;
            player.Experience.Reset();
            player.Statistics.Restat();

            player.SkillTree.Reskill();
            player.SkillTree.ResetAvailableSkillPoints();
            for (int levelNr = 2; levelNr <= player.Level; levelNr++)
            {
                player.SkillTree.AddSkillPointsForLevelUp(levelNr, false);
            }

            if (player.Job.Id != job.Id)
            {
                player.ChangeJob(job.Id);
            }

            player.UpdateCache();

            using var snapshot = new SetExperienceSnapshot(player);
            player.Send(snapshot);
            player.SendToVisible(snapshot);
        }

        private bool TryParseJob(string jobInput, out JobData job)
        {
            if (jobInput.IsNumeric())
            {
                var jobId = jobInput.ToEnum<DefineJob.Job>();

                job = _gameResources.Jobs.Values.FirstOrDefault(x => x.Id == jobId);
            }
            else
            {
                job = _gameResources.Jobs.Values.FirstOrDefault(x => x.Id.ToString().Equals(jobInput, StringComparison.OrdinalIgnoreCase));

                if (job == null)
                {
                    return false;
                }
            }

            return true;
        }

        private int GetLevel(string levelInput)
        {
            if (!int.TryParse(levelInput, out int level))
            {
                throw new InvalidOperationException($"Failed to parse level input: '{levelInput}'");
            }

            if (level <= 0)
            {
                throw new InvalidOperationException($"Level cannot be equal or smaller than 0. ({level})");
            }

            return level;
        }
    }
}