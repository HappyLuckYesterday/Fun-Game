using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage an entity's messenger contacts.
    /// </summary>
    public interface IMessenger : IPacketSerializer
    {
        /// <summary>
        /// Gets or sets the current messenger status.
        /// </summary>
        MessengerStatusType Status { get; set; }

        /// <summary>
        /// Gets or sets the friend list.
        /// </summary>
        IContactList Friends { get; }
    }
}
