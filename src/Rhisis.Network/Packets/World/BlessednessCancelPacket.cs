﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="BlessednessCancelPacket"/> structure.
    /// </summary>
    public struct BlessednessCancelPacket : IEquatable<BlessednessCancelPacket>
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public int Item { get; set; }

        /// <summary>
        /// Creates a new <see cref="BlessednessCancelPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public BlessednessCancelPacket(INetPacketStream packet)
        {
            this.Item = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="BlessednessCancelPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="BlessednessCancelPacket"/></param>
        public bool Equals(BlessednessCancelPacket other)
        {
            return this.Item == other.Item;
        }
    }
}