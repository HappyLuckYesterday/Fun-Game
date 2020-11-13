namespace Rhisis.Game.Abstractions.Features.Chat
{
    /// <summary>
    /// Provides a mechanism to manage the entities chat system.
    /// </summary>
    public interface IChat
    {
        /// <summary>
        /// Makes the given world object speak the given message to all visible objects.
        /// </summary>
        /// <param name="text">Text to be sent as normal chat message.</param>
        void Speak(string text);

        /// <summary>
        /// Makes the given world object shout the given message to all visible objects.
        /// </summary>
        /// <param name="text">Text to be sent as shouting chat message.</param>
        void Shout(string text);
    }
}
