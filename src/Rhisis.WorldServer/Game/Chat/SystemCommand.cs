using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets;
using System;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/system", AuthorityType.GameMaster)]
    [ChatCommand("/sys", AuthorityType.GameMaster)]
    public class SystemChatCommand : IChatCommand
    {
        private readonly ILogger<SystemChatCommand> _logger;
        private readonly IWorldServer _worldServer;

        /// <summary>
        /// Creates a new <see cref="SystemChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public SystemChatCommand(ILogger<SystemChatCommand> logger, IWorldServer worldServer)
        {
            _logger = logger;
            _worldServer = worldServer;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (parameters.Length < 1)
            {
                throw new ArgumentException("You must have at least one parameter to send a message.");
            }

            string message = $"[{player.Name}] " + string.Join(" ", parameters);

            using (var systemMessagePacket = new SystemMessagePacket(message.TakeCharacters(511)))
            {
                _worldServer.SendToAll(systemMessagePacket);
            }

            _logger.LogTrace($"Player '{player.Name}' sent the following system message : '{message}'.");
        }
    }
}