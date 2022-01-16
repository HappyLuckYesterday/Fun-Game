using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class PlayerAnglePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the Angle.
        /// </summary>
        public float Angle { get; private set; }

        /// <summary>
        /// Gets the X angle.
        /// </summary>
        public float AngleX { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Angle = packet.ReadSingle();
            AngleX = packet.ReadSingle();
        }
    }
}