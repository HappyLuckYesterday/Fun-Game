namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage the player's experience.
    /// </summary>
    public interface IExperience
    {
        /// <summary>
        /// Gets the amount of experience.
        /// </summary>
        long Amount { get; }

        /// <summary>
        /// Increases the experience of a player.
        /// </summary>
        /// <param name="amount">Amount of experience to increase.</param>
        /// <returns>True if the player's experience has increased; false otherwise.</returns>
        bool Increase(long amount);

        /// <summary>
        /// Decreases the experience of a player.
        /// </summary>
        /// <param name="amount">Amount of experience to decrease.</param>
        /// <returns>True if the player's experience has decreased; false otherwise.</returns>
        bool Decrease(long amount);
    }
}
