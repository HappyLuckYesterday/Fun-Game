﻿using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Features.Chat
{
    /// <summary>
    /// Provides a mechanism to create chat commands.
    /// </summary>
    public interface IChatCommand
    {
        /// <summary>
        /// Executes the chat command.
        /// </summary>
        /// <param name="parameters">Chat commands parameters.</param>
        void Execute(IPlayer player, object[] parameters);
    }
}