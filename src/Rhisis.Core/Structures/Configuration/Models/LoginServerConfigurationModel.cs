using Newtonsoft.Json;

namespace Rhisis.Core.Structures.Configuration.Models;

public class LoginServerConfigurationModel
{
    [JsonProperty(PropertyName = ConfigurationSections.Login)]
    public LoginOptions LoginConfiguration { get; set; }
    [JsonProperty(PropertyName = ConfigurationSections.Core)]
    public CoreOptions CoreConfiguration { get; set; }
}
