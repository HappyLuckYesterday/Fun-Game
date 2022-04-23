using Newtonsoft.Json;
using Rhisis.Core.Structures.Configuration.World;

namespace Rhisis.Core.Structures.Configuration.Models
{
    public class WorldServerConfigurationModel
    {
        [JsonProperty(PropertyName = ConfigurationSections.World)]
        public WorldOptions WorldConfiguration { get; set; }

        [JsonProperty(PropertyName = ConfigurationSections.Core)]
        public CoreOptions CoreConfiguration { get; set; }
    }
}
