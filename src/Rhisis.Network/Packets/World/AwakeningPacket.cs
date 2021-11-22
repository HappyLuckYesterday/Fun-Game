using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class AwakeningPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public int Item { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Item = packet.Read<int>();
        }
    }
}