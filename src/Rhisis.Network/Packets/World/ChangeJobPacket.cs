﻿using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
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
        public void Deserialize(ILitePacketStream packet)
        {
            Job = packet.Read<int>();
            Gama = packet.Read<int>() == 1;
        }
    }
}