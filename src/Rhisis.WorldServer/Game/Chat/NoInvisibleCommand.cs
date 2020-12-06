using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Snapshots;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/noinvisible", AuthorityType.GameMaster)]
    [ChatCommand("/noinv", AuthorityType.GameMaster)]
    public class NoInvisibleChatCommand : IChatCommand
    {
        private readonly ILogger<NoInvisibleChatCommand> _logger;

        /// <summary>
        /// Creates a new <see cref="NoInvisibleChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public NoInvisibleChatCommand(ILogger<NoInvisibleChatCommand> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (player.Mode.HasFlag(ModeType.TRANSPARENT_MODE))
            {
                player.Mode &= ~ModeType.TRANSPARENT_MODE;

                using (var snapshots = new FFSnapshot())
                {
                    snapshots.Merge(new ModifyModeSnapshot(player, player.Mode));
                    snapshots.Merge(new DestPositionSnapshot(player));
                    snapshots.Merge(new DestAngleSnapshot(player));

                    player.Send(snapshots);
                    player.SendToVisible(snapshots);
                }

                _logger.LogTrace($"Player '{player.Name}' is not anymore invisible.");
            }
            else
            {
                _logger.LogTrace($"Player '{player.Name}' is currently not invisible.");
            }
        }
    }
}