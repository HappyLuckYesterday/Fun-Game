using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol.Snapshots;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/invisible", AuthorityType.GameMaster)]
[ChatCommand("/inv", AuthorityType.GameMaster)]
internal sealed class InvisibleCommand : IChatCommand
{
    public void Execute(Player player, object[] parameters)
    {
        if (!player.Mode.HasFlag(ModeType.TRANSPARENT_MODE))
        {
            player.Mode |= ModeType.TRANSPARENT_MODE;

            using (var snapshot = new ModifyModeSnapshot(player, player.Mode))
            {
                player.Send(snapshot);
                player.SendToVisible(snapshot);
            }
        }
    }
}
