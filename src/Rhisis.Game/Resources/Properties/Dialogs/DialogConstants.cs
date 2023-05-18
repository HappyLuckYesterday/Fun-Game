using Rhisis.Game.Common;
using System.Collections.Generic;

namespace Rhisis.Game.Resources.Properties.Dialogs;

public static class DialogConstants
{
    public static readonly string Bye = "BYE";
    public static readonly string Yes = "__YES__";
    public static readonly string No = "__NO__";
    public static readonly string Cancel = "__CANCEL__";
    public static readonly string Ok = "__OK__";

    public static readonly IEnumerable<DialogLink> QuestAcceptDeclineButtons = new List<DialogLink>
    {
        new DialogLink(QuestState.BeginYes.ToString(), Yes),
        new DialogLink(QuestState.BeginNo.ToString(), No)
    };
    public static readonly IEnumerable<DialogLink> QuestOkButtons = new List<DialogLink>
    {
        new DialogLink(Bye, Ok, 0)
    };
    public static readonly IEnumerable<DialogLink> QuestFinishButtons = new List<DialogLink>
    {
        new DialogLink(QuestState.EndCompleted.ToString(), Ok)
    };
}