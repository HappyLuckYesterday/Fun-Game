using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Rhisis.Core.Configuration.Cluster;

public sealed class DefaultCharacterOptions
{
    [ConfigurationKeyName("level")]
    public int Level { get; set; }

    [ConfigurationKeyName("gold")]
    public int Gold { get; set; }

    [ConfigurationKeyName("strength")]
    public int Strength { get; set; }

    [ConfigurationKeyName("stamina")]
    public int Stamina { get; set; }

    [ConfigurationKeyName("dexterity")]
    public int Dexterity { get; set; }

    [ConfigurationKeyName("intelligence")]
    public int Intelligence { get; set; }

    [ConfigurationKeyName("map")]
    public int MapId { get; set; }

    [ConfigurationKeyName("pos-x")]
    public float PositionX { get; set; }

    [ConfigurationKeyName("pos-y")]
    public float PositionY { get; set; }

    [ConfigurationKeyName("pos-z")]
    public float PositionZ { get; set; }

    [ConfigurationKeyName("equiped")]
    public DefaultCharacterEquipedItems EquipedItems { get; set; } = new();

    [ConfigurationKeyName("inventory")]
    public IEnumerable<DefaultInventoryItem> InventoryItems { get; set; } = new HashSet<DefaultInventoryItem>();
}
