using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Represents the messenger configuration section.
    /// </summary>
    [DataContract]
    public sealed class MessengerConfiguration
    {
        /// <summary>
        /// Gets or sets the maximum allowed amount of friends in the messenger.
        /// </summary>
        [DataMember(Name = "maximumFriends")]
        public int MaximumFriends { get; set; } = 200;
    }
}
