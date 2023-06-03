using Rhisis.Core.Extensions;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using System;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/level", AuthorityType.GameMaster)]
[ChatCommand("/lv", AuthorityType.GameMaster)]
internal sealed class LevelCommand : IChatCommand
{
    public void Execute(Player player, object[] parameters)
    {
        if (parameters.Length < 2)
        {
            player.SendSnoopMessage("/Missing arguments. /level [job] [level] (legend)");
            return;
        }

        string jobParameter = parameters[0].ToString();
        string levelParameter = parameters[1].ToString();

        if (!TryParseJob(jobParameter, out JobProperties job))
        {
            throw new InvalidOperationException($"Failed to parse job: '{jobParameter}'.");
        }

        player.Level = Math.Clamp(GetLevel(levelParameter), job.MinLevel, job.MaxLevel);
        player.DeathLevel = 0;
        player.Experience.Reset();
        player.ResetStatistics();
        player.ResetSkills();
        player.ResetAvailableSkillPoints();
        
        for (int levelNr = 2; levelNr <= player.Level; levelNr++)
        {
            player.Skills.AddSkillPointsForLevelUp(levelNr, false);
        }

        player.ChangeJob(job.Id);

        using SetExperienceSnapshot snapshot = new(player);
        player.SendToVisible(snapshot, sendToSelf: true);
    }

    private static bool TryParseJob(string jobInput, out JobProperties job)
    {
        if (jobInput.IsNumeric())
        {
            var jobId = jobInput.ToEnum<DefineJob.Job>();

            job = GameResources.Current.Jobs.Get(jobId);
        }
        else
        {
            job = GameResources.Current.Jobs.Find(x => x.Id.ToString().Equals(jobInput, StringComparison.OrdinalIgnoreCase));

            if (job == null)
            {
                return false;
            }
        }

        return true;
    }

    private static int GetLevel(string levelInput)
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
