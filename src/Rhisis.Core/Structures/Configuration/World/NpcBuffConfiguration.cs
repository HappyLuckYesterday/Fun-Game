using System.Collections.Generic;

namespace Rhisis.Core.Structures.Configuration.World
{
    public class NpcBuffConfiguration
    {
        /// <summary>
        /// Gets or sets the buffing NPC name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the minimum player level that the NPC can buff.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// Gets or sets the maxmimum player levle that the NPC can buff.
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        /// Gets a value that indicates if the current NPC buff configuration is the default one.
        /// </summary>
        public bool IsDefault => string.IsNullOrWhiteSpace(Name);

        /// <summary>
        /// Gets or sets a collection of the buffs to apply.
        /// </summary>
        public IEnumerable<NpcBuffDescriptor> Buffs { get; set; }
    }
}
