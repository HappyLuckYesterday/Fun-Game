using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Provides a data structure for configuring the buff pang system.
    /// </summary>
    public class NpcBuffConfigurationSection
    {
        /// <summary>
        /// Gets or sets the default npc buff configuration.
        /// </summary>
        public NpcBuffConfiguration Default { get; set; }

        /// <summary>
        /// Gets or sets the additionnal npc buff configuration.
        /// </summary>
        public IEnumerable<NpcBuffConfiguration> Additionnal { get; set; }

        /// <summary>
        /// Gets the npc buff configuration based on the given name.
        /// </summary>
        /// <param name="name">NPC name key.</param>
        /// <returns>Buff configuration or default configuration.</returns>
        public NpcBuffConfiguration Get(string name) 
            => Additionnal.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? Default;
    }
}
