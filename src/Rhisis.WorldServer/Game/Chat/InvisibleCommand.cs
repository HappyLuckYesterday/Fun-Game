using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/invisible", AuthorityType.GameMaster)]
    [ChatCommand("/inv", AuthorityType.GameMaster)]
    public class InvisibleChatCommand : IChatCommand
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
        public void Execute(IPlayer player, object[] parameters)
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
}