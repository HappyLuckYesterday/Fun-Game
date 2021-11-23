using LiteNetwork.Protocol.Abstractions;
using System;

namespace Rhisis.Protocol.Abstractions
{
    /// <summary>
    /// Provides an interface to explorer flyff packets.
    /// </summary>
    public interface IFFPacket : ILitePacketStream, IDisposable
    {
        /// <summary>
        /// Write packet header.
        /// </summary>
        /// <param name="packetHeader">FFPacket header</param>
        void WriteHeader(object packetHeader);
    }
}
