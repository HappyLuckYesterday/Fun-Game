using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

public sealed class CustomizationOptions
{
    /// <summary>
    /// Gets or sets the cost of the face change.
    /// </summary>
    [ConfigurationKeyName("face-cost")]
    public int FaceCost { get; set; }

    /// <summary>
    /// Gets or sets the cost of the hair change.
    /// </summary>
    [ConfigurationKeyName("hair-cost")]
    public int HairCost { get; set; }

    /// <summary>
    /// Gets or sets the cost of the hair color change.
    /// </summary>
    [ConfigurationKeyName("hair-color-cost")]
    public int HairColorCost { get; set; }
}