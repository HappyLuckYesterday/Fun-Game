﻿using Rhisis.Game.Common;
using System.Runtime.Serialization;

namespace Rhisis.Game.Resources.Properties;

[DataContract]
public class ShopItemProperties
{
    /// <summary>
    /// Gets the item Id.
    /// </summary>
    [DataMember(Name = "itemId")]
    public int Id { get; protected set; }

    /// <summary>
    /// Gets or sets the item refine.
    /// </summary>
    [DataMember(Name = "refine")]
    public byte Refine { get; set; }

    /// <summary>
    /// Gets or sets the item element. (Fire, water, electricity, etc...)
    /// </summary>
    [DataMember(Name = "element")]
    public ElementType Element { get; set; }

    /// <summary>
    /// Gets or sets the item element refine.
    /// </summary>
    [DataMember(Name = "elementRefine")]
    public byte ElementRefine { get; set; }
}
