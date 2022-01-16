using Rhisis.Abstractions.Protocol;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class SetTargetPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target id.
        /// </summary>
        public uint TargetId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether target should be cleared or not.
        /// </summary>
        public TargetModeType TargetMode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            TargetId = packet.ReadUInt32();
            TargetMode = (TargetModeType)packet.ReadByte();
        }
    }
}