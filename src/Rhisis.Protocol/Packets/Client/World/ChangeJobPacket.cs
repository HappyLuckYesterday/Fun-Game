using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ChangeJobPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the job.
        /// </summary>
        public int Job { get; private set; }

        /// <summary>
        /// I have no idea what gama is. Probably game?
        /// </summary>
        public bool Gama { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Job = packet.Read<int>();
            Gama = packet.Read<int>() == 1;
        }
    }
}