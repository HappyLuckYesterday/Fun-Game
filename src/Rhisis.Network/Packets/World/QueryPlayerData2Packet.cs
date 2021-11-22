﻿using System.Collections.Generic;
using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class QueryPlayerData2Packet : IPacketDeserializer
    {
        /// <summary>
        /// Gets the size of the list.
        /// </summary>
        public uint Size { get; private set; }

        /// <summary>
        /// Gets the player id and version.
        /// </summary>
        public Dictionary<uint, int> PlayerDictionary { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            PlayerDictionary = new Dictionary<uint, int>();
            Size = packet.Read<uint>();
            for (uint i = 0; i < Size; i++)
                PlayerDictionary.Add(packet.Read<uint>(), packet.Read<int>());
        }
    }
}
