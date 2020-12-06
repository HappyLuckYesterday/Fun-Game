using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using System;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/summon", AuthorityType.GameMaster)]
    [ChatCommand("/su", AuthorityType.GameMaster)]
    public class SummonCommand : IChatCommand
    {
        private readonly IWorldServer _worldServer;
        private readonly ILogger<SummonCommand> _logger;

        /// <summary>
        /// Creates a new <see cref="SummonCommand"/> instance.
        /// </summary>
        /// <param name="logger">logger system.</param>
        /// <param name="worldServer">World server.</param>
        public SummonCommand(ILogger<SummonCommand> logger, IWorldServer worldServer)
        {
            _logger = logger;
            _worldServer = worldServer;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (parameters.Length == 1)
            {
                IPlayer playerToSummon = _worldServer.GetPlayerEntity(parameters[0].ToString());

                if (playerToSummon == null)
                {
                    throw new ArgumentException($"The player doesn't exist or is not connected.", nameof(parameters));
                }

                playerToSummon.Teleport(player.Position, player.Map.Id);
                _logger.LogTrace($"{playerToSummon.Name} is summoned by {player.Name}.");
            }
            else
            {
                throw new ArgumentException("Too many or not enough arguments.");
            }
        }
    }
}