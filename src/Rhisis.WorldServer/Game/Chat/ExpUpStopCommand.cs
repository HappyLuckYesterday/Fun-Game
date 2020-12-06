using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/ExpUpStop", AuthorityType.Administrator)]
    [ChatCommand("/es", AuthorityType.Administrator)]
    public class ExpUpStopChatCommand : IChatCommand
    {
        private readonly ILogger<ExpUpStopChatCommand> _logger;

        /// <summary>
        /// Creates a new <see cref="ExpUpStopChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public ExpUpStopChatCommand(ILogger<ExpUpStopChatCommand> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (!player.Mode.HasFlag(ModeType.MODE_EXPUP_STOP))
            {
                player.Mode |= ModeType.MODE_EXPUP_STOP;
                _logger.LogTrace($"Player '{player.Name}' is now in Exp Up Stop mode.");
            }
            else
            {
                player.Mode &= ~ModeType.MODE_EXPUP_STOP;
                _logger.LogTrace($"Player '{player.Name}' isn't in Exp Up Stop mode.");
            }
        }
    }
}