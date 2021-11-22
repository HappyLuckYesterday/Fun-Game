using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class OpenShopWindowPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the selected object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            ObjectId = packet.Read<uint>();
        }
    }
}
