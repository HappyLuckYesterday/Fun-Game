﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Duel
{
    /// <summary>
    /// Defines the <see cref="PartyDuelRequestPacket"/> structure.
    /// </summary>
    public struct PartyDuelRequestPacket : IEquatable<PartyDuelRequestPacket>
    {
        /// <summary>
        /// Gets the source player id.
        /// </summary>
        public uint SourcePlayerId { get; set; }

        /// <summary>
        /// Gets the destination player id.
        /// </summary>
        public uint DestinationPlayerId { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyDuelRequestPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyDuelRequestPacket(INetPacketStream packet)
        {
            this.SourcePlayerId = packet.Read<uint>();
            this.DestinationPlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PartyDuelRequestPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyDuelRequestPacket"/></param>
        public bool Equals(PartyDuelRequestPacket other)
        {
            return this.SourcePlayerId == other.SourcePlayerId && this.DestinationPlayerId == other.DestinationPlayerId;
        }
    }
}