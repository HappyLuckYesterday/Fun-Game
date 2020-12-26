namespace Rhisis.Core.Structures.Configuration.World
{
    public class NpcBuffDescriptor
    {
        /// <summary>
        /// Gets or sets the buff skill id.
        /// </summary>
        /// <remarks>
        /// This field can either be the defined skill string like "SI_ASS_CHEER_HEAPUP" or its numerical id.
        /// </remarks>
        public string SkillId { get; set; }

        /// <summary>
        /// Gets or sets the buff skill level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the buff time in seconds.
        /// </summary>
        public int Time { get; set; }
    }
}
