﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ScriptAddGoldPacket"/> structure.
    /// </summary>
    public struct ScriptAddGoldPacket : IEquatable<ScriptAddGoldPacket>
    {
        /// <summary>
        /// Gets the gold.
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// Creates a new <see cref="ScriptAddGoldPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ScriptAddGoldPacket(INetPacketStream packet)
        {
            this.Gold = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="ScriptAddGoldPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ScriptAddGoldPacket"/></param>
        public bool Equals(ScriptAddGoldPacket other)
        {
            return this.Gold == other.Gold;
        }
    }
}