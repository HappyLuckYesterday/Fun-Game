using Rhisis.Game.Abstractions.Entities;
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

        /// <summary>
        /// Sends a friendship request to the given player.
        /// </summary>
        /// <param name="targetPlayer">Target player to send the friendship request.</param>
        void SendFriendRequest(IPlayer targetPlayer);

        /// <summary>
        /// Adds a player to the friend contact list.
        /// </summary>
        /// <param name="playerToAdd">Player to add as a friend.</param>
        void AddFriend(IPlayer playerToAdd);

        /// <summary>
        /// Notifies the current player a friend with the given player id has connected.
        /// </summary>
        /// <param name="playerId">Player id.</param>
        /// <param name="statusType">Friend status type.</param>
        void OnFriendConnected(int playerId, MessengerStatusType statusType);
    }
}
