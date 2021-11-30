using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class DoUseItemTargetPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the material.
        /// </summary>
        public uint Material { get; private set; }

        /// <summary>
        /// Gets the target.
        /// </summary>
        public uint Target { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Material = packet.Read<uint>();
            Target = packet.Read<uint>();
        }
    }
}