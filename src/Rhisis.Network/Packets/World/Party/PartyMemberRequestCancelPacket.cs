﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartyMemberRequestCancelPacket"/> structure.
    /// </summary>
    public struct PartyMemberRequestCancelPacket : IEquatable<PartyMemberRequestCancelPacket>
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; set; }

        /// <summary>
        /// Gets the member id.
        /// </summary>
        public uint MemberId { get; set; }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyMemberRequestCancelPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyMemberRequestCancelPacket(INetPacketStream packet)
        {
            this.LeaderId = packet.Read<uint>();
            this.MemberId = packet.Read<uint>();
            this.Mode = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="PartyMemberRequestCancelPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyMemberRequestCancelPacket"/></param>
        public bool Equals(PartyMemberRequestCancelPacket other)
        {
            return this.LeaderId == other.LeaderId && this.MemberId == other.MemberId && this.Mode == other.Mode;
        }
    }
}