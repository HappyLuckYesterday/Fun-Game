using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Bank
{
    public class GetGoldBankPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; private set; }

        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        public uint Gold { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Slot = packet.Read<byte>();
            Gold = packet.Read<uint>();
        }
    }
}