using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Game.Abstractions.Protocol
{
    /// <summary>
    /// Provides an interface to deserialize an object.
    /// </summary>
    public interface IPacketDeserializer
    {
        /// <summary>
        /// Deserializes the current packet stream.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        void Deserialize(ILitePacketStream packet);
    }
}
