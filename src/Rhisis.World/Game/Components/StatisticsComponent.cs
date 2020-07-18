namespace Rhisis.World.Game.Components
{
    public class StatisticsComponent
    {
        /// <summary>
        /// Gets or sets the available statistics points.
        /// </summary>
        public ushort AvailablePoints { get; set; }

        public ushort SkillPoints { get; set; }

        /// <summary>
        /// Gets or sets the original strength points.
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        /// Gets or sets the original stamina points.
        /// </summary>
        public int Stamina { get; set; }

        /// <summary>
        /// Gets or sets the original dexterity points.
        /// </summary>
        public int Dexterity { get; set; }

        /// <summary>
        /// Gets or sets the orginal intelligence points.
        /// </summary>
        public int Intelligence { get; set; }
    }
}
