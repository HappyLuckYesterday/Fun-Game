using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;
using System;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/nofreeze", AuthorityType.GameMaster)]
    [ChatCommand("/nofr", AuthorityType.GameMaster)]
    public class NoFreezeChatCommand : IChatCommand
    {
        private readonly ILogger<NoFreezeChatCommand> _logger;
        private readonly IWorldServer _worldServer;

        /// <summary>
        /// Creates a new <see cref="NoFreezeChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldServer">World server system.</param>
        public NoFreezeChatCommand(ILogger<NoFreezeChatCommand> logger, IWorldServer worldServer)
        {
            _logger = logger;
            _worldServer = worldServer;
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