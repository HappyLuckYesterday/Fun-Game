using Rhisis.Game;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rhisis.WorldServer.Handlers
{
    /// <summary>
    /// Handles every packets related to the chat system.
    /// </summary>
    [Handler]
    public sealed class ChatHandler
    {
        private readonly IChatCommandManager _chatCommandManager;

        public ChatHandler(IChatCommandManager chatCommandManager)
        {
            _chatCommandManager = chatCommandManager;
        }

        /// <summary>
        /// Handles a chat request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.CHAT)]
        public void OnChat(IPlayer player, ChatPacket packet)
        {
            if (!string.IsNullOrEmpty(packet.Message))
            {
                if (packet.Message.StartsWith(GameConstants.ChatCommandPrefix))
                {
                    (string, string[]) commandInfo = GetCommandParameters(packet.Message);
                    IChatCommand chatCommand = _chatCommandManager.GetChatCommand(commandInfo.Item1, player.Authority);

                    if (chatCommand == null)
                    {
                        throw new ArgumentException($"Cannot find chat command: '/{commandInfo.Item1}'", nameof(chatCommand));
                    }

                    chatCommand.Execute(player, commandInfo.Item2);
                }
                else
                {
                    player.Chat.Speak(packet.Message);
                }
            }
        }

        /// <summary>
        /// Gets the command parameters.
        /// </summary>
        /// <param name="command">Command line</param>
        /// <param name="commandName">Command name</param>
        /// <returns></returns>
        private (string, string[]) GetCommandParameters(string command)
        {
            string commandName = command.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            if (string.IsNullOrEmpty(commandName))
            {
                return (command, Enumerable.Empty<string>().ToArray());
            }

            string commandParameters = command.Remove(0, commandName.Length);

            return (commandName, Regex.Matches(commandParameters, @"[\""].+?[\""]|[^ ]+").Select(m => m.Value.Trim('"')).ToArray());
        }
    }
}
