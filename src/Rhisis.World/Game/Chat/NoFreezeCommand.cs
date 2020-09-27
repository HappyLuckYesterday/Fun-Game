using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.World.Game.Entities;
using System;
using Rhisis.World.Packets;
using Rhisis.Game.Common;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using System.Collections.Generic;
using System.Linq;
using Rhisis.Network.Snapshots;

namespace Rhisis.World.Game.Chat
{
    [ChatCommand("/nofreeze", AuthorityType.GameMaster)]
    [ChatCommand("/nofr", AuthorityType.GameMaster)]
    public class NoFreezeChatCommand : IChatCommand
    {
        private readonly ILogger<NoFreezeChatCommand> _logger;
        private readonly IWorldServer _worldServer;
        private readonly IPlayerDataPacketFactory _playerDataPacketFactory;

        /// <summary>
        /// Creates a new <see cref="NoFreezeChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldServer">World server system.</param>
        /// <param name="playerDataPacketFactory">Player data packet factory system.</param>
        public NoFreezeChatCommand(ILogger<NoFreezeChatCommand> logger, IWorldServer worldServer, IPlayerDataPacketFactory playerDataPacketFactory)
        {
            _logger = logger;
            _worldServer = worldServer;
            _playerDataPacketFactory = playerDataPacketFactory;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (parameters.Length == 1) 
            {
                IPlayer playerToUnfreeze = _worldServer.GetPlayerEntity(parameters[0].ToString());
                
                if (playerToUnfreeze.Mode.HasFlag(ModeType.DONMOVE_MODE))
                {
                    playerToUnfreeze.Mode &= ~ ModeType.DONMOVE_MODE;

                    using (var snapshot = new ModifyModeSnapshot(player, player.Mode))
                    {
                        player.Send(snapshot);
                        player.SendToVisible(snapshot);
                    }

                    _logger.LogTrace($"Player '{playerToUnfreeze.Name}' is not freezed anymore.");
                }
                else 
                {
                    _logger.LogTrace($"Player '{playerToUnfreeze.Name}' is currently not freezed.");
                }
            }
            else
            {
                throw new ArgumentException("Too many or not enough arguments.");
            }
        }
    }
}