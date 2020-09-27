using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;
using Rhisis.World.Packets;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/noonekill", AuthorityType.GameMaster)]
    [ChatCommand("/nook", AuthorityType.GameMaster)]
    public class NoOneKillChatCommand : IChatCommand
    {
        private readonly ILogger<NoOneKillChatCommand> _logger;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;

        /// <summary>
        /// Creates a new <see cref="NoOneKillChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="playerDataPacketFactory">Player data packet factory system.</param>
        public NoOneKillChatCommand(ILogger<NoOneKillChatCommand> logger, IPlayerDataPacketFactory playerDataPacketFactory)
        {
            _logger = logger;
            _playerDataPacketFactory = playerDataPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (player.Mode.HasFlag(ModeType.ONEKILL_MODE))
            {
                player.Mode &= ~ModeType.ONEKILL_MODE;

                using (var snapshot = new ModifyModeSnapshot(player, player.Mode))
                {
                    player.Send(snapshot);
                    player.SendToVisible(snapshot);
                }

                _logger.LogTrace($"Player '{player.Name}' is not anymore in OneKill mode.");
            }
            else
            {
                _logger.LogTrace($"Player '{player.Name}' is currently not in OneKill mode.");
            }
        }
    }
}