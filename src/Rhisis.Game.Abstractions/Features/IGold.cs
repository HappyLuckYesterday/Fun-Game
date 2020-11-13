namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage the player's gold.
    /// </summary>
    public interface IGold
    {
        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        int Amount { get; }

        /// <summary>
        /// Decrease the gold amount.
        /// </summary>
        /// <param name="amount">Amount of gold to remove.</param>
        /// <returns>True if the gold amount has been decreased; false otherwise.</returns>
        bool Decrease(int amount);

        /// <summary>
        /// Increases the gold amount.
        /// </summary>
        /// <param name="amount">Amount of gold to add.</param>
        /// <returns>True if the gold amount has been increased; false otherwise.</returns>
        bool Increase(int amount);
    }
}
