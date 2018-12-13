﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="AwakeningPacket"/> structure.
    /// </summary>
    public struct AwakeningPacket : IEquatable<AwakeningPacket>
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public int Item { get; set; }

        /// <summary>
        /// Creates a new <see cref="AwakeningPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public AwakeningPacket(INetPacketStream packet)
        {
            this.Item = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="AwakeningPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="AwakeningPacket"/></param>
        public bool Equals(AwakeningPacket other)
        {
            return this.Item == other.Item;
        }
    }
}