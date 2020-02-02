﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Represents the World Server configuration structure.
    /// </summary>
    [DataContract]
    public class WorldConfiguration
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
        public WorldRates Rates { get; set; } = new WorldRates();

        /// <summary>
        /// Gets or sets the world drops configuration.
        /// </summary>
        [DataMember(Name = "drops")]
        public WorldDrops Drops { get; set; } = new WorldDrops();

        /// <summary>
        /// Gets or sets the mails configuration.
        /// </summary>
        [DataMember(Name = "mails")]
        public MailConfiguration Mails { get; set; } = new MailConfiguration();

        /// <summary>
        /// Gets or sets the mails configuration.
        /// </summary>
        [DataMember(Name = "perin")]
        public PerinConfiguration Perin { get; set; } = new PerinConfiguration();

        /// <summary>
        /// Gets or sets the Style Customization settings.
        /// </summary>
        [DataMember(Name = "customization")]
        public StyleCustomization Customization { get; set; } = new StyleCustomization();

        /// <summary>
        /// Gets or sets the Party Configuration settings.
        /// </summary>
        [DataMember(Name = "party")]
        public PartyConfiguration PartyConfiguration { get; set; } = new PartyConfiguration();

        /// <summary>
        /// Gets or sets the death configuration settings.
        /// </summary>
        [DataMember(Name = "death")]
        public DeathConfiguration Death { get; set; } = new DeathConfiguration();

        /// <summary>
        /// Creates a new <see cref="WorldConfiguration"/> instance.
        /// </summary>
        public WorldConfiguration()
        {
            Systems = new Dictionary<string, bool>();
        }
    }
}
