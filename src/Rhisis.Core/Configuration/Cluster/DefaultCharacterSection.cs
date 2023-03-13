using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration.Cluster;

public sealed class DefaultCharacterSection
{
    [ConfigurationKeyName("man")]
    public DefaultCharacterOptions Man { get; set; }

    [ConfigurationKeyName("woman")]
    public DefaultCharacterOptions Woman { get; set; }
}
