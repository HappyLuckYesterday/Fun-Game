using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/onekill", AuthorityType.GameMaster)]
    [ChatCommand("/ok", AuthorityType.GameMaster)]
    public class OneKillChatCommand : IChatCommand
    {
        private readonly ILogger<OneKillChatCommand> _logger;

        /// <summary>
        /// Creates a new <see cref="OneKillChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public OneKillChatCommand(ILogger<OneKillChatCommand> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (!player.Mode.HasFlag(ModeType.ONEKILL_MODE))
            {
                player.Mode |= ModeType.ONEKILL_MODE;

                using (var snapshot = new ModifyModeSnapshot(player, player.Mode))
                {
                    player.Send(snapshot);
                    player.SendToVisible(snapshot);
                }

                _logger.LogTrace($"Player '{player.Name}' is now in OneKill mode.");
            }
            else
            {
                _logger.LogTrace($"Player '{player.Name}' is already in OneKill mode.");
            }
        }
    }
}