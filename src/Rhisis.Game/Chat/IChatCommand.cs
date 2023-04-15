using Rhisis.Game.Entities;

namespace Rhisis.Game.Chat;

/// <summary>
/// Provides a mechanism to create chat commands.
/// </summary>
public interface IChatCommand
{
    /// <summary>
    /// Gets the command parameter parsing mode.
    /// </summary>
    ChatParameterParsingType ParsingType => ChatParameterParsingType.Default;

    /// <summary>
    /// Executes the chat command.
    /// </summary>
    /// <param name="parameters">Chat commands parameters.</param>
    void Execute(Player player, object[] parameters);
}