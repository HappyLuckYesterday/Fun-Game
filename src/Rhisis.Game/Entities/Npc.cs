using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources.Properties;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rhisis.Game.Entities;

[DebuggerDisplay("{Name}")]
public sealed class Npc : WorldObject
{
    private static float OralTextRadisu = 50f;

    private long _lastSpeakTime;

    public override WorldObjectType Type => WorldObjectType.Mover;

    public NpcProperties Properties { get; }
 
    public Npc(NpcProperties properties)
    {
        Properties = properties;
        Name = Properties.Id;
    }

    public void Update()
    {
        if (!IsSpawned)
        {
            return;
        }

        Speak();
    }

    private void Speak()
    {
        if (Properties.Dialog is not null && !string.IsNullOrEmpty(Properties.Dialog.OralText))
        {
            if (_lastSpeakTime <= Time.TimeInSeconds())
            {
                IEnumerable<Player> playersAround = VisibleObjects
                    .Where(x => x is Player && Position.IsInCircle(x.Position, OralTextRadisu))
                    .Cast<Player>();

                if (playersAround.Any())
                {
                    foreach (Player player in playersAround)
                    {
                        using ChatSnapshot packet = new(this, Properties.Dialog.OralText);

                        player.Send(packet);
                    }
                }

                _lastSpeakTime = Time.TimeInSeconds() + FFRandom.Random(10, 15);
            }
        }
    }
}