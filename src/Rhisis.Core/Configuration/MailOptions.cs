using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

public sealed class MailOptions
{
    /// <summary>
    /// Gets or sets the mail shipping cost.
    /// </summary>
    [ConfigurationKeyName("shipping-cost")]
    public int ShippingCost { get; set; }
}
