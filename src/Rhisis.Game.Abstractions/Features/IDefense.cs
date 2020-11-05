namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage an entity's defense.
    /// </summary>
    public interface IDefense
    {
        /// <summary>
        /// Gets the minimum defense.
        /// </summary>
        int Minimum { get; }

        /// <summary>
        /// Gets the maximum defense.
        /// </summary>
        int Maximum { get; }

        /// <summary>
        /// Gets the entity's defense based on the minimum and maximum defense.
        /// </summary>
        /// <returns></returns>
        int GetDefense();

        /// <summary>
        /// Updates and calculates the entity's defense.
        /// </summary>
        void Update();
    }
}
