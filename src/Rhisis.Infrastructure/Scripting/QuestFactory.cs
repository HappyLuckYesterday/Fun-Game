using NLua;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.Infrastructure.Scripting.Quests;

namespace Rhisis.Infrastructure.Scripting
{
    public static class QuestFactory
    {
        public static IQuestScript CreateQuest(int questId, string questName, LuaTable luaScriptTable)
        {
            return new QuestScript(questId, questName, luaScriptTable);
        }
    }
}
