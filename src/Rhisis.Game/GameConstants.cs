using Rhisis.Game.Common;
using System.Collections.Generic;

namespace Rhisis.Game
{
    public static class GameConstants
    {
        public const short DefaultObjectSize = 100;

        public const string ChatCommandPrefix = "/";

        /// <summary>
        /// Skill points usage based on the job type.
        /// </summary>
        public static readonly IReadOnlyDictionary<DefineJob.JobType, int> SkillPointUsage = new Dictionary<DefineJob.JobType, int>
        {
            { DefineJob.JobType.JTYPE_BASE, 1 },
            { DefineJob.JobType.JTYPE_EXPERT, 2 },
            { DefineJob.JobType.JTYPE_PRO, 3 },
            { DefineJob.JobType.JTYPE_MASTER, 3 },
            { DefineJob.JobType.JTYPE_HERO, 3 }
        };
    }
}
