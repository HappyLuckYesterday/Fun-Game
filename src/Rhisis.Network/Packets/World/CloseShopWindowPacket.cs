using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class CloseShopWindowPacket : IPacketDeserializer
    {
        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            throw new System.NotImplementedException();
        }
    }
}