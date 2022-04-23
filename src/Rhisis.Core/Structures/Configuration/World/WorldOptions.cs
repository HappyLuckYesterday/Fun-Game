using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Represents the World Server configuration structure.
    /// </summary>
    [DataContract]
    public class WorldOptions
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        [DataMember(Name = "host")]
        [DefaultValue("127.0.0.1")]
        [Display(Name = "World server host address", Order = 0)]
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        [DataMember(Name = "port")]
        [DefaultValue(5400)]
        [Display(Name = "World server listening port", Order = 1)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the world's id.
        /// </summary>
        [DataMember(Name = "id")]
        [Required]
        [Display(Name = "World server unique ID", Order = 2)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent cluster server id.
        /// </summary>
        [DataMember(Name = "clusterId")]
        [Required]
        [Display(Name = "World server parent cluster ID", Order = 3)]
        public int ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the world's name.
        /// </summary>
        [DataMember(Name = "name")]
        [Display(Name = "World server name (Channel name)", Order = 4)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the enabled or disabled systems of the world server.
        /// </summary>
        [DataMember(Name = "systems")]
        public IDictionary<string, bool> Systems { get; set; }

        /// <summary>
        /// Gets or sets the maps of the world server.
        /// </summary>
        [DataMember(Name = "maps")]
        public IEnumerable<string> Maps { get; set; }

        /// <summary>
        /// Gets or sets the world server's language.
        /// </summary>
        [DataMember(Name = "language")]
        [DefaultValue("en")]
        [Display(Name = "World server language", Order = 5)]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the world server's rates.
        /// </summary>
        [DataMember(Name = "rates")]
        public WorldRatesOptions Rates { get; set; } = new WorldRatesOptions();

        /// <summary>
        /// Gets or sets the world drops configuration.
        /// </summary>
        [DataMember(Name = "drops")]
        public WorldDropsOptions Drops { get; set; } = new WorldDropsOptions();

        /// <summary>
        /// Gets or sets the mails configuration.
        /// </summary>
        [DataMember(Name = "mails")]
        public MailOptions Mails { get; set; } = new MailOptions();

        /// <summary>
        /// Gets or sets the mails configuration.
        /// </summary>
        [DataMember(Name = "perin")]
        public PerinOptions Perin { get; set; } = new PerinOptions();

        /// <summary>
        /// Gets or sets the Style Customization settings.
        /// </summary>
        [DataMember(Name = "customization")]
        public StyleCustomizationOptions Customization { get; set; } = new StyleCustomizationOptions();

        /// <summary>
        /// Gets or sets the Party Configuration settings.
        /// </summary>
        [DataMember(Name = "party")]
        public PartyOptions PartyConfiguration { get; set; } = new PartyOptions();

        /// <summary>
        /// Gets or sets the death configuration settings.
        /// </summary>
        [DataMember(Name = "death")]
        public DeathOptions Death { get; set; } = new DeathOptions();

        /// <summary>
        /// Gets or sets the messenger configuration settings.
        /// </summary>
        [DataMember(Name = "messenger")]
        public MessengerOptions Messenger { get; set; } = new MessengerOptions();

        /// <summary>
        /// Gets or sets the NPC buff configuration settings.
        /// </summary>
        [DataMember(Name = "npcBuff")]
        public NpcBuffConfigurationSection NpcBuff { get; set; } = new NpcBuffConfigurationSection();

        /// <summary>
        /// Gets or sets the cluster cache configuration.
        /// </summary>
        public ClusterCacheOptions ClusterCache { get; set; } = new ClusterCacheOptions();

        /// <summary>
        /// Creates a new <see cref="WorldOptions"/> instance.
        /// </summary>
        public WorldOptions()
        {
            Systems = new Dictionary<string, bool>();
        }
    }
}
