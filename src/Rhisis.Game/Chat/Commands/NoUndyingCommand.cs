using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol.Snapshots;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/noundying", AuthorityType.GameMaster)]
[ChatCommand("/noud", AuthorityType.GameMaster)]
internal sealed class NoUndyingCommand : IChatCommand
{
    private readonly ILogger<NoUndyingCommand> _logger;

    /// <summary>
    /// Creates a new <see cref="NoUndyingCommand"/> instance.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public NoUndyingCommand(ILogger<NoUndyingCommand> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public void Execute(Player player, object[] parameters)
    {
        if (player.Mode.HasFlag(ModeType.MATCHLESS_MODE))
        {
            player.Mode &= ~ModeType.MATCHLESS_MODE;

            using (var snapshot = new ModifyModeSnapshot(player, player.Mode))
            {
                player.Send(snapshot);
                player.SendToVisible(snapshot);
            }

            _logger.LogTrace($"Player '{player.Name}' is not anymore in undying mode.");
        }
        else
        {
            _logger.LogTrace($"Player '{player.Name}' is currently not in undying mode.");
        }
    }
}