using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides an interface that defines a contact in the contact list.
    /// </summary>
    public interface IContact
    {
        /// <summary>
        /// Gets the contact player object id.
        /// </summary>
        uint Id { get; }
        
        /// <summary>
        /// Gets the contact channel id.
        /// </summary>
        int Channel { get; }

        /// <summary>
        /// Gets or sets a boolean value that indicates the friend blocking date.
        /// </summary>
        bool IsBlocked { get; set; }

        /// <summary>
        /// Gets the contact name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the contact status.
        /// </summary>
        MessengerStatusType Status { get; set; }

        /// <summary>
        /// Gets the contact job.
        /// </summary>
        DefineJob.Job Job { get; }
    }
}
