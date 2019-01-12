﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the World Server configuration structure.
    /// </summary>
    [DataContract]
    public class WorldConfiguration : BaseConfiguration
    {
        public const string DefaultLanguage = "en";
        public const int DefaultMailShippingCost = 500;
        public const int DefaultChangeFaceCost = 1000000;
        public const int DefaultChangeHairColorCost = 4000000;
        public const int DefaultChangeHairCost = 2000000;

        /// <summary>
        /// Gets or sets the world's id.
        /// </summary>
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent cluster server id.
        /// </summary>
        [DataMember(Name = "clusterId")]
        public int ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the world's name.
        /// </summary>
        [DataMember(Name = "name")]
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
        /// Gets or sets the IPC configuration.
        /// </summary>
        [DataMember(Name = "isc")]
        public ISCConfiguration ISC { get; set; }

        /// <summary>
        /// Gets or sets mail shipping costs.
        /// </summary>
        [DataMember(Name = "mailShippingCost")]
        public uint MailShippingCost { get; set; }

        /// <summary>
        /// Gets or sets the costs for changing characters face.
        /// </summary>
        [DataMember(Name = "changeFaceCost")]
        public uint ChangeFaceCost { get; set; }

        /// <summary>
        /// Gets or sets the costs for changing characters hair.
        /// </summary>
        [DataMember(Name = "changeHairCost")]
        public uint ChangeHairCost { get; set; }

        /// <summary>
        /// Gets or sets the costs for changing characters hair color.
        /// </summary>
        [DataMember(Name = "changeHairColorCost")]
        public uint ChangeHairColorCost { get; set; }

        /// <summary>
        /// Creates a new <see cref="WorldConfiguration"/> instance.
        /// </summary>
        public WorldConfiguration()
        {
            this.Language = DefaultLanguage;
            this.Systems = new Dictionary<string, bool>();
            this.ISC = new ISCConfiguration();
            this.Rates = new WorldRates();
            this.MailShippingCost = DefaultMailShippingCost;
            this.ChangeFaceCost = DefaultChangeFaceCost;
            this.ChangeHairCost = DefaultChangeHairCost;
            this.ChangeHairColorCost = DefaultChangeHairColorCost;
        }
    }
}
