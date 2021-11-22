﻿using LiteNetwork.Protocol.Abstractions;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Protocol;

namespace Rhisis.Network.Packets.World
{
    public class TeleSkillPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the position
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Position = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
        }
    }
}