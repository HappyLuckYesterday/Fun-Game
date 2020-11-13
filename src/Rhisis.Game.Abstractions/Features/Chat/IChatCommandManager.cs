using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Features.Chat
{
    /// <summary>
    /// Provides a mechanism to manage the chat commands.
    /// </summary>
    public interface IChatCommandManager
    {
        /// <summary>
        /// Loads all chat commands.
        /// </summary>
        void Load();

        /// <summary>
        /// Gets a chat command by its command name.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <param name="authority">Executor authority.</param>
        IChatCommand GetChatCommand(string command, AuthorityType authority);
    }
}