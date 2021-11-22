using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class ScriptAddGoldPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the gold.
        /// </summary>
        public int Gold { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Gold = packet.Read<int>();
        }
    }
}