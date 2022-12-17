using Newtonsoft.Json;

namespace Rhisis.Core.Structures.Configuration.Models;

public class ClusterServerConfigurationModel
{
    [JsonProperty(PropertyName = ConfigurationSections.Cluster)]
    public ClusterOptions ClusterServerConfiguration { get; set; }

    [JsonProperty(PropertyName = ConfigurationSections.Core)]
    public CoreOptions CoreConfiguration { get; set; }
}
