﻿using NLua;
using Rhisis.Core.Extensions;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.Scripting.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Scripting.Quests
{
    internal class QuestStartRequirements : IQuestStartRequirements
    {
        /// <inheritdoc />
        public int MinLevel { get; }

        /// <inheritdoc />
        public int MaxLevel { get; }

        /// <inheritdoc />
        public IEnumerable<DefineJob.Job> Jobs { get; }

        /// <inheritdoc />
        public string PreviousQuestId { get; }

        /// <summary>
        /// Creates a new <see cref="QuestStartRequirements"/> instance.
        /// </summary>
        /// <param name="questStartRequirementsTable">Quest requirements lua table.</param>
        internal QuestStartRequirements(LuaTable questStartRequirementsTable)
        {
            MinLevel = questStartRequirementsTable.GetValue<int>(QuestScriptConstants.MinLevel);
            MaxLevel = questStartRequirementsTable.GetValue<int>(QuestScriptConstants.MaxLevel);
            Jobs = (questStartRequirementsTable[QuestScriptConstants.Job] as LuaTable)?.Values
                .ToArray<string>()
                .Select(x => (DefineJob.Job)Enum.Parse(typeof(DefineJob.Job), x));
            PreviousQuestId = questStartRequirementsTable.GetValue<string>(QuestScriptConstants.PreviousQuest);
        }
    }
}
