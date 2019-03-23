﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ErrorPacket"/> structure.
    /// </summary>
    public struct ErrorPacket : IEquatable<ErrorPacket>
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public int Data { get; set; }

        /// <summary>
        /// Creates a new <see cref="ErrorPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ErrorPacket(INetPacketStream packet)
        {
            this.Code = packet.Read<int>();
            this.Data = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="ErrorPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ErrorPacket"/></param>
        public bool Equals(ErrorPacket other)
        {
            return this.Code == other.Code && this.Data == other.Data;
        }
    }
}