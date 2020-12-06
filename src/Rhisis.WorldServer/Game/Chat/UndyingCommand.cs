using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/undying", AuthorityType.GameMaster)]
    [ChatCommand("/ud", AuthorityType.GameMaster)]
    public class UndyingChatCommand : IChatCommand
    {
        private readonly ILogger<UndyingChatCommand> _logger;

        /// <summary>
        /// Creates a new <see cref="UndyingChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public UndyingChatCommand(ILogger<UndyingChatCommand> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
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
}