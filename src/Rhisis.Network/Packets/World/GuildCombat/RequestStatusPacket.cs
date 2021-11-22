using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    public class RequestStatusPacket : IPacketDeserializer
    {
        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            throw new System.NotImplementedException();
        }
    }
}