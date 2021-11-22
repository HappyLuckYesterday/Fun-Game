﻿using LiteNetwork.Protocol.Abstractions;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Protocol;

namespace Rhisis.Network.Packets.World
{
    public class LocalPosFromIAPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the local position.
        /// </summary>
        public Vector3 LocalPosition { get; private set; }

        /// <summary>
        /// Gets the id of the IA.
        /// </summary>
        public uint IAId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            LocalPosition = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
            IAId = packet.Read<uint>();
        }
    }
}