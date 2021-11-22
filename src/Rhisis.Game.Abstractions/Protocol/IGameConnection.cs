using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Game.Abstractions.Protocol
{
    /// <summary>
    /// Provides an abstraction for a game connection.
    /// </summary>
    public interface IGameConnection
    {
        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        uint SessionId { get; }

        /// <summary>
        /// Sends a packet to the current connection.
        /// </summary>
        /// <param name="packet"></param>
        void Send(ILitePacketStream packet);
    }
}
