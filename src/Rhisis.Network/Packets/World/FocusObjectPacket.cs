﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="FocusObjectPacket"/> structure.
    /// </summary>
    public struct FocusObjectPacket : IEquatable<FocusObjectPacket>
    {
        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; set; }

        /// <summary>
        /// Creates a new <see cref="FocusObjectPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public FocusObjectPacket(INetPacketStream packet)
        {
            this.ObjectId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="FocusObjectPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="FocusObjectPacket"/></param>
        public bool Equals(FocusObjectPacket other)
        {
            return this.ObjectId == other.ObjectId;
        }
    }
}