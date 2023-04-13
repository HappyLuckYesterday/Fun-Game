using Rhisis.Game.Common;
using Rhisis.Game.Resources.Properties;
using System.Diagnostics;

namespace Rhisis.Game.Entities;

[DebuggerDisplay("{Name}")]
public sealed class Npc : WorldObject
{
    public override WorldObjectType Type => WorldObjectType.Mover;

    public NpcProperties Properties { get; }
 
    public Npc(NpcProperties properties)
    {
        Properties = properties;
        Name = Properties.Id;
    }
}