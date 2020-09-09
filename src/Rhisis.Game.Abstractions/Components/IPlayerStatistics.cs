namespace Rhisis.Game.Abstractions.Components
{
    public interface IPlayerStatistics : IStatistics
    {
        /// <summary>
        /// Gets or sets the available statistics points.
        /// </summary>
        ushort AvailablePoints { get; set; }

        /// <summary>
        /// Update the player's statistics.
        /// </summary>
        /// <param name="strength">New strength points.</param>
        /// <param name="stamina">New stamina points.</param>
        /// <param name="dexterity">New dexterity points.</param>
        /// <param name="intelligence">New intelligence points.</param>
        void UpdateStatistics(int strength, int stamina, int dexterity, int intelligence);

        /// <summary>
        /// Reset the player's statistics to default values.
        /// </summary>
        void Restat();
    }
}
