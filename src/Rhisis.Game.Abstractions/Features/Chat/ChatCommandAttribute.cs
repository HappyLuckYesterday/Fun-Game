using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Abstractions.Features.Chat
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ChatCommandAttribute : Attribute
    {
        /// <summary>
        /// Gets the chat command.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Gets the chat commands minimum authorization.
        /// </summary>
        public AuthorityType MinimumAuthorization { get; }

        /// <summary>
        /// Creates a new <see cref="ChatCommandAttribute"/> instance.
        /// </summary>
        /// <param name="command">Chat command</param>
        /// <param name="minimumAuthorization">Minimum Authorization</param>
        public ChatCommandAttribute(string command, AuthorityType minimumAuthorization)
        {
            Command = command;
            MinimumAuthorization = minimumAuthorization;
        }
    }
}