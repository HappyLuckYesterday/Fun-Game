namespace Rhisis.Game.Abstractions.Features.Chat
{
    /// <summary>
    /// Provides different modes for handling chat command parameters.
    /// </summary>
    public enum ChatParameterParsingType
    {
        /// <summary>
        /// The default parsing mode.
        /// It will use <see cref="string.Split(char[])"/> method to split the command parameters.
        /// </summary>
        Default,

        /// <summary>
        /// The parameters will be treated as plain text.
        /// </summary>
        PlainText
    }
}