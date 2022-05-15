using Rhisis.Core.Structures;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class LocalPosFromIAPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the local position.
        /// </summary>
        public Vector3 LocalPosition { get; private set; }

        /// <summary>
        /// Gets the id of the IA.
        /// </summary>
        public uint IAId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            LocalPosition = new Vector3(packet.ReadSingle(), packet.ReadSingle(), packet.ReadSingle());
            IAId = packet.ReadUInt32();
        }
    }
}