using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Protocol;
using Rhisis.Protocol.Snapshots;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/noinvisible", AuthorityType.GameMaster)]
[ChatCommand("/noinv", AuthorityType.GameMaster)]
internal sealed class NoInvisibleCommand : IChatCommand
{
    /// <inheritdoc />
    public void Execute(Player player, object[] parameters)
    {
        if (player.Mode.HasFlag(ModeType.TRANSPARENT_MODE))
        {
            player.Mode &= ~ModeType.TRANSPARENT_MODE;

            using (var snapshots = new FFSnapshot())
            {
                snapshots.Merge(new ModifyModeSnapshot(player, player.Mode));
                snapshots.Merge(new DestPositionSnapshot(player));
                snapshots.Merge(new DestAngleSnapshot(player));

                player.Send(snapshots);
                player.SendToVisible(snapshots);
            }
        }
    }
}