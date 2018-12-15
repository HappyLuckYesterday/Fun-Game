﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Packet
{
    /// <summary>
    /// Defines the <see cref="PartyChangeNamePacket"/> structure.
    /// </summary>
    public struct PartyChangeNamePacket : IEquatable<PartyChangeNamePacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyChangeNamePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyChangeNamePacket(INetPacketStream packet)
        {
            this.PlayerId = packet.Read<uint>();
            this.Name = packet.Read<string>();
        }

        /// <summary>
        /// Compares two <see cref="PartyChangeNamePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyChangeNamePacket"/></param>
        public bool Equals(PartyChangeNamePacket other)
        {
            return this.PlayerId == other.PlayerId && this.Name == other.Name;
        }
    }
}