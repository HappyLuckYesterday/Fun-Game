using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Game.Abstractions.Protocol
{
    /// <summary>
    /// Provides an interface to serialize an object.
    /// </summary>
    public interface IPacketSerializer
    {
        /// <summary>
        /// Serializes the current object.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        void Serialize(ILitePacketStream packet);
    }
}
