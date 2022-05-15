using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class JoinPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the World Id.
        /// </summary>
        public int WorldId { get; private set; }

        /// <summary>
        /// Gets the Player Id.
        /// </summary>
        public int PlayerId { get; private set; }

        /// <summary>
        /// Gets the Authentication key.
        /// </summary>
        public int AuthenticationKey { get; private set; }

        /// <summary>
        /// Gets the player's party id.
        /// </summary>
        public int PartyId { get; private set; }

        /// <summary>
        /// Gets the player's guild id.
        /// </summary>
        public int GuildId { get; private set; }

        /// <summary>
        /// Gets the player's guild war id.
        /// </summary>
        public int GuildWarId { get; private set; }

        /// <summary>
        /// Gets the Id of multi
        /// </summary>
        /// <remarks>
        /// What is this ?
        /// </remarks>
        public int IdOfMulti { get; private set; }

        /// <summary>
        /// Gets the player's slot.
        /// </summary>
        public byte Slot { get; private set; }

        /// <summary>
        /// Gets the player's name.
        /// </summary>
        public string PlayerName { get; private set; }

        /// <summary>
        /// Gets the player's account username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets the player's account password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Gets the messenger state.
        /// </summary>
        public int MessengerState { get; private set; }

        /// <summary>
        /// Gets the messenger count.
        /// </summary>
        public int MessengerCount { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            WorldId = packet.ReadInt32();
            PlayerId = packet.ReadInt32();
            AuthenticationKey = packet.ReadInt32();
            PartyId = packet.ReadInt32();
            GuildId = packet.ReadInt32();
            GuildWarId = packet.ReadInt32();
            IdOfMulti = packet.ReadInt32(); // what is this?
            Slot = packet.ReadByte();
            PlayerName = packet.ReadString();
            Username = packet.ReadString();
            Password = packet.ReadString();
            MessengerState = packet.ReadInt32();
            MessengerCount = packet.ReadInt32();
        }
    }
}
