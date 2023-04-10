using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration.Cluster;

public sealed class DefaultCharacterEquipedItems
{
    [ConfigurationKeyName("hat")]
    public string Hat { get; set; }

    [ConfigurationKeyName("body")]
    public string Body { get; set; }

    [ConfigurationKeyName("hand")]
    public string Hand { get; set; }

    [ConfigurationKeyName("boots")]
    public string Boots { get; set; }

    [ConfigurationKeyName("right-weapon")]
    public string RightWeapon { get; set; }

    [ConfigurationKeyName("left-weapon")]
    public string LeftWeapon { get; set; }
}
