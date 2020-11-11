using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System.Linq;

namespace Rhisis.Network.Snapshots
{
    public class DefinedTextSnapshot : FFSnapshot
    {
        public DefinedTextSnapshot(IPlayer player, DefineText textId, params object[] parameters)
            : base(parameters.Any() ? SnapshotType.DEFINEDTEXT : SnapshotType.DEFINEDTEXT1, player.Id)
        {
            WriteInt32((int)textId);
            WriteString(string.Join(" ", parameters));
        }
    }
}
