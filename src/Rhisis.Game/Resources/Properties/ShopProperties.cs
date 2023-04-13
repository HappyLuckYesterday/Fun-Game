using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhisis.Game.Resources.Properties;

/// <summary>
/// Represents a NPC Shop data.
/// </summary>
[DataContract]
public class ShopProperties
{
    /// <summary>
    /// Gets the default shop tab count.
    /// </summary>
    public static readonly int DefaultTabCount = 4;

    /// <summary>
    /// Gets or sets the shop name.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the shop items.
    /// </summary>
    [DataMember(Name = "items")]
    public List<ShopItemProperties>[] Items { get; set; }

    /// <summary>
    /// Creates a new <see cref="ShopProperties"/> instance.
    /// </summary>
    /// <param name="shopName">Npc shop name</param>
    /// <param name="shopTabs">Npc shop tabs data</param>
    public ShopProperties(string shopName, int shopTabs = DefaultTabCount)
    {
        Name = shopName;
        Items = new List<ShopItemProperties>[shopTabs];

        for (var i = 0; i < shopTabs; i++)
        {
            Items[i] = new List<ShopItemProperties>();
        }
    }
}
