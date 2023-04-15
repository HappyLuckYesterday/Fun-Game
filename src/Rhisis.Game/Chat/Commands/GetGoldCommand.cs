using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/getgold", AuthorityType.Administrator)]
[ChatCommand("/gg", AuthorityType.Administrator)]
internal sealed class GetGoldCommand : IChatCommand
{
    private readonly ILogger<GetGoldCommand> _logger;

    /// <summary>
    /// Creates a new <see cref="GetGoldCommand"/> instance.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public GetGoldCommand(ILogger<GetGoldCommand> logger)
    {
        _logger = logger;
    }

    public void Execute(Player player, object[] parameters)
    {
        if (parameters.Length == 1 && int.TryParse(parameters[0].ToString(), out int gold))
        {
            if (!player.Gold.Increase(gold))
            {
                _logger.LogTrace($"Failed to create {gold} for player '{player.Name}'.");
            }
            else
            {
                _logger.LogTrace($"Player '{player.Name}' created {gold} gold.");
            }
        }
    }
}
