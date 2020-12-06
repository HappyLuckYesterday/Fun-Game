using Microsoft.Extensions.Logging;
using System;
using Rhisis.Game.Common;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network.Snapshots;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/freeze", AuthorityType.GameMaster)]
    [ChatCommand("/fr", AuthorityType.GameMaster)]
    public class FreezeChatCommand : IChatCommand
    {
        private readonly ILogger<FreezeChatCommand> _logger;
        private readonly IWorldServer _worldServer;

        /// <summary>
        /// Creates a new <see cref="FreezeChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldServer">World server system.</param>
        public FreezeChatCommand(ILogger<FreezeChatCommand> logger, IWorldServer worldServer)
        {
            _logger = logger;
            _worldServer = worldServer;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (parameters.Length == 1) 
            {
                IPlayer playerToFreeze = _worldServer.GetPlayerEntity(parameters[0].ToString());

                if (!player.Mode.HasFlag(ModeType.DONMOVE_MODE))
                {
                    playerToFreeze.Mode |= ModeType.DONMOVE_MODE;

                    using (var snapshot = new ModifyModeSnapshot(playerToFreeze, playerToFreeze.Mode))
                    {
                        player.Send(snapshot);
                        player.SendToVisible(snapshot);
                    }

                    _logger.LogTrace($"Player '{playerToFreeze.Name}' is now freezed.");
                }
                else 
                {
                    _logger.LogTrace($"Player '{playerToFreeze.Name}' is already freezed.");
                }
            }
            else
            {
                throw new ArgumentException("Too many or not enough arguments.");
            }
        }
    }
}