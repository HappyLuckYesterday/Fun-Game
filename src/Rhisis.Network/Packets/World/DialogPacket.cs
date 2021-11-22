﻿using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class DialogPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the dialog owner object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <summary>
        /// Gets the dialog key.
        /// </summary>
        public string DialogKey { get; private set; }

        /// <summary>
        /// Gets the first parameter.
        /// </summary>
        /// <remarks>
        /// Figure out what this value is.
        /// </remarks>
        public int Param { get; private set; }

        /// <summary>
        /// Gets the dialog quest id.
        /// </summary>
        public int QuestId { get; private set; }

        /// <summary>
        /// Gets the quest mover id.
        /// </summary>
        public uint MoverId { get; private set; }

        /// <summary>
        /// Gets the quest player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            ObjectId = packet.Read<uint>();
            DialogKey = packet.Read<string>();
            Param = packet.Read<int>();
            QuestId = packet.Read<int>();
            MoverId = packet.Read<uint>();
            PlayerId = packet.Read<uint>();
        }
    }
}
