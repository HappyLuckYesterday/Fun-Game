using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Bank
{
    public class ChangeBankPasswordPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the old password.
        /// </summary>
        public string OldPassword { get; private set; }

        /// <summary>
        /// Gets the new password.
        /// </summary>
        public string NewPassword { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            OldPassword = packet.Read<string>();
            NewPassword = packet.Read<string>();
            Id = packet.Read<uint>();
            ItemId = packet.Read<uint>();
        }
    }
}