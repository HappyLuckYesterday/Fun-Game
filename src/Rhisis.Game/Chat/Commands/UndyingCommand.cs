using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol.Snapshots;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/undying", AuthorityType.GameMaster)]
[ChatCommand("/ud", AuthorityType.GameMaster)]
internal sealed class UndyinCommand : IChatCommand
{
    private readonly ILogger<UndyinCommand> _logger;

    /// <summary>
    /// Creates a new <see cref="UndyinCommand"/> instance.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public UndyinCommand(ILogger<UndyinCommand> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public void Execute(Player player, object[] parameters)
    {
        if (!player.Mode.HasFlag(ModeType.MATCHLESS_MODE))
        {
            player.Mode |= ModeType.MATCHLESS_MODE;

            using (var snapshot = new ModifyModeSnapshot(player, player.Mode))
            {
                player.Send(snapshot);
                player.SendToVisible(snapshot);
            }

            _logger.LogTrace($"Player '{player.Name}' is now in undying mode.");
        }
        else
        {
            _logger.LogTrace($"Player '{player.Name}' is already in undying mode.");
        }
    }
}