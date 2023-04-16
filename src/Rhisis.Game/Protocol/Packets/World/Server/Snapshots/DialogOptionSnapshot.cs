using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class DialogOptionSnapshot : FFSnapshot
{
    public DialogOptionSnapshot(WorldObject worldObject, DialogOptions option)
        : base(SnapshotType.RUNSCRIPTFUNC, worldObject.ObjectId)
    {
        WriteInt16((short)option);
    }

    public DialogOptionSnapshot(WorldObject worldObject, DialogOptions option, string dialogText, int questId = 0)
        : base(SnapshotType.RUNSCRIPTFUNC, worldObject.ObjectId)
    {
        WriteInt16((short)option);
        WriteString(dialogText);
        WriteInt32(questId);
    }

    public DialogOptionSnapshot(WorldObject worldObject, DialogOptions option, string dialogLinkTitle, string dialogLinkKey, int questId = 0)
        : base(SnapshotType.RUNSCRIPTFUNC, worldObject.ObjectId)
    {
        WriteInt16((short)option);
        WriteString(dialogLinkTitle);
        WriteString(dialogLinkKey);
        WriteInt32(0);
        WriteInt32(questId);
    }
}
