using Microsoft.Extensions.Configuration;

namespace Rhisis.Core.Configuration;

public sealed class MessengerOptions
{
    /// <summary>
    /// Gets or sets the maximum amount of friends.
    /// </summary>
    [ConfigurationKeyName("maximum-friends")]
    public int MaximumFriends { get; set; }
}
