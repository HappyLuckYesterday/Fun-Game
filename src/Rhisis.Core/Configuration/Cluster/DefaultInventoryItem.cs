using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration.Cluster;

public sealed class DefaultInventoryItem
{
    [ConfigurationKeyName("item")]
    public string ItemId { get; set; }

    [ConfigurationKeyName("refine")]
    public byte Refine { get; set; }

    [ConfigurationKeyName("element")]
    public byte Element { get; set; }

    [ConfigurationKeyName("element-refine")]
    public byte ElementRefine { get; set; }

    [ConfigurationKeyName("quantity")]
    public int Quantity { get; set; }
}