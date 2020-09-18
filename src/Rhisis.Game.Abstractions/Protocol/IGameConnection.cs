using Sylver.Network.Common;
using Sylver.Network.Data;

namespace Rhisis.Game.Abstractions.Protocol
{
    /// <summary>
    /// Provides an abstraction for a game connection.
    /// </summary>
    public interface IGameConnection : INetUser
    {
        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        uint SessionId { get; }

        /// <summary>
        /// Sends the packet to every connected players arround the current player.
        /// </summary>
        /// <param name="packet">Packet to send.</param>
        /// <param name="sendToPlayer">
        /// Boolean value that indicates if the packet should also be sent to the current player.
        /// </param>
        void SendToVisible(INetPacketStream packet, bool sendToPlayer = false);
    }
}
