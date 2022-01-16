using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots
{
    public class DialogOptionSnapshot : FFSnapshot
    {
        public DialogOptionSnapshot(IWorldObject worldObject, DialogOptions option)
            : base(SnapshotType.RUNSCRIPTFUNC, worldObject.Id)
        {
            Write((short)option);
        }

        public DialogOptionSnapshot(IWorldObject worldObject, DialogOptions option, string dialogText, int questId = 0)
            : base(SnapshotType.RUNSCRIPTFUNC, worldObject.Id)
        {
            Write((short)option);
            Write(dialogText);
            Write(questId);
        }

        public DialogOptionSnapshot(IWorldObject worldObject, DialogOptions option, string dialogLinkTitle, string dialogLinkKey, int questId = 0)
            : base(SnapshotType.RUNSCRIPTFUNC, worldObject.Id)
        {
            Write((short)option);
            Write(dialogLinkTitle);
            Write(dialogLinkKey);
            Write(0);
            Write(questId);
        }
    }
}
