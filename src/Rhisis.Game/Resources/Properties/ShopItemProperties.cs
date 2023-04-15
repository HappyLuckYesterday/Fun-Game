using Rhisis.Game.Common;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Rhisis.Game.Resources.Properties;

[DebuggerDisplay("ShopItem: {Id} +{Refine} {Element}+{ElementRefine}")]
public class ShopItemProperties
{
    /// <summary>
    /// Gets the item Id.
    /// </summary>
    [JsonPropertyName("itemId")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the item refine.
    /// </summary>
    [JsonPropertyName("refine")]
    public byte Refine { get; set; }

    /// <summary>
    /// Gets or sets the item element. (Fire, water, electricity, etc...)
    /// </summary>
    [JsonPropertyName("element")]
    public ElementType Element { get; set; }

    /// <summary>
    /// Gets or sets the item element refine.
    /// </summary>
    [JsonPropertyName("elementRefine")]
    public byte ElementRefine { get; set; }
}
