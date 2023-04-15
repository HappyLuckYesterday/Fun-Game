using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Rhisis.Game.Resources.Properties;

/// <summary>
/// Represents a NPC Shop data.
/// </summary>
[DebuggerDisplay("Shop: {Name}")]
public class ShopProperties
{
    /// <summary>
    /// Gets or sets the shop name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the shop items.
    /// </summary>
    [JsonPropertyName("items")]
    public List<ShopItemProperties>[] Items { get; set; }
}
