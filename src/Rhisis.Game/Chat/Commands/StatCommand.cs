using System;
using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol.Snapshots;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/stat", AuthorityType.GameMaster)]
internal sealed class StatCommand : IChatCommand
{
    private readonly ILogger<StatCommand> _logger;

    /// <summary>
    /// Creates a new <see cref="StatCommand"/> instance.
    /// </summary>
    /// <param name="logger">Logger.</param>
    public StatCommand(ILogger<StatCommand> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public void Execute(Player player, object[] parameters)
    {
        if (parameters.Length <= 0)
        {
            throw new ArgumentException($"Set stat command must have at least one parameter.", nameof(parameters));
        }

        long attributeId = parameters.Length >= 1 ? Convert.ToInt64(parameters[0]) : 0;
        long quantity = parameters.Length >= 2 ? Convert.ToInt64(parameters[1]) : 0;

        if (attributeId <= 0 || attributeId > 4)
        {
            throw new ArgumentException($"Set stat command attributeId is not an attribute.", nameof(parameters));
        }

        DefineAttributes attribute = (DefineAttributes)attributeId;

        switch (attribute)
        {
            case DefineAttributes.DST_STR:
                player.Statistics.Strength = (int)quantity;
                break;
            case DefineAttributes.DST_STA:
                player.Statistics.Stamina = (int)quantity;
                break;
            case DefineAttributes.DST_DEX:
                player.Statistics.Dexterity = (int)quantity;
                break;
            case DefineAttributes.DST_INT:
                player.Statistics.Intelligence = (int)quantity;
                break;
            default:
                break;
        }

        _logger.LogTrace($"Player {player.Name} update attribute {attribute} value to {quantity}");

        using (var snapshot = new ModifyStateSnapshot(player))
        {
            player.Send(snapshot);
            player.SendToVisible(snapshot, true);
        }
    }
}