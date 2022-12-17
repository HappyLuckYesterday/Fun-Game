using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots;

public class DialogOptionSnapshot : FFSnapshot
{
    public DialogOptionSnapshot(IWorldObject worldObject, DialogOptions option)
        : base(SnapshotType.RUNSCRIPTFUNC, worldObject.Id)
    {
        WriteInt16((short)option);
    }

    public DialogOptionSnapshot(IWorldObject worldObject, DialogOptions option, string dialogText, int questId = 0)
        : base(SnapshotType.RUNSCRIPTFUNC, worldObject.Id)
    {
        WriteInt16((short)option);
        WriteString(dialogText);
        WriteInt32(questId);
    }

    public DialogOptionSnapshot(IWorldObject worldObject, DialogOptions option, string dialogLinkTitle, string dialogLinkKey, int questId = 0)
        : base(SnapshotType.RUNSCRIPTFUNC, worldObject.Id)
    {
        WriteInt16((short)option);
        WriteString(dialogLinkTitle);
        WriteString(dialogLinkKey);
        WriteInt32(0);
        WriteInt32(questId);
    }
}
