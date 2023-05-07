using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol.Snapshots;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/invisible", AuthorityType.GameMaster)]
[ChatCommand("/inv", AuthorityType.GameMaster)]
internal sealed class InvisibleChatCommand : IChatCommand
{
    private readonly ILogger<InvisibleChatCommand> _logger;

    /// <summary>
    /// Creates a new <see cref="InvisibleChatCommand"/> instance.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public InvisibleChatCommand(ILogger<InvisibleChatCommand> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
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

            _logger.LogTrace($"Player '{player.Name}' is now invisible.");
        }
        else
        {
            _logger.LogTrace($"Player '{player.Name}' is already invisible.");
        }
    }
}