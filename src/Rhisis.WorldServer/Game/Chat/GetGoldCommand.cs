using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using System;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/getgold", AuthorityType.Administrator)]
    [ChatCommand("/gg", AuthorityType.Administrator)]
    public class GetGoldChatCommand : IChatCommand
    {
        private readonly ILogger<GetGoldChatCommand> _logger;

        /// <summary>
        /// Creates a new <see cref="GetGoldChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public GetGoldChatCommand(ILogger<GetGoldChatCommand> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (parameters.Length == 1)
            {
                int gold = Convert.ToInt32(parameters[0]);

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
}