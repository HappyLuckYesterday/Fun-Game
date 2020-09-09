namespace Rhisis.Game.Abstractions.Components
{
    public interface IStatistics
    {
        /// <summary>
        /// Gets or sets the original strength points.
        /// </summary>
        int Strength { get; set; }

        /// <summary>
        /// Gets or sets the original stamina points.
        /// </summary>
        int Stamina { get; set; }

        /// <summary>
        /// Gets or sets the original dexterity points.
        /// </summary>
        int Dexterity { get; set; }

        /// <summary>
        /// Gets or sets the orginal intelligence points.
        /// </summary>
        int Intelligence { get; set; }
    }
}
