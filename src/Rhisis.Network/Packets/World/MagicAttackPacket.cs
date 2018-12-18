﻿using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="MagicAttackPacket"/> structure.
    /// </summary>
    public struct MagicAttackPacket : IEquatable<MagicAttackPacket>
    {
        /// <summary>
        /// Gets the attack message.
        /// </summary>
        public ObjectMessageType AttackMessage { get; }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; }

        /// <summary>
        /// Gets the second parameter.
        /// </summary>
        public int Parameter2 { get; set; }

        /// <summary>
        /// Gets the third parameter.
        /// </summary>
        public int Parameter3 { get; set; }

        /// <summary>
        /// Gets the magic power.
        /// </summary>
        public int MagicPower { get; }

        /// <summary>
        /// Gets the id of the hit SFX.
        /// </summary>
        public int IdSfxHit { get; set; }

        public MagicAttackPacket(INetPacketStream packet)
        {
            this.AttackMessage = (ObjectMessageType)packet.Read<uint>();
            this.ObjectId = packet.Read<uint>();
            this.Parameter2 = packet.Read<int>();
            this.Parameter3 = packet.Read<int>();
            this.MagicPower = packet.Read<int>();
            this.IdSfxHit = packet.Read<int>();
        }

        public bool Equals(MagicAttackPacket other)
        {
            return this.AttackMessage == other.AttackMessage &&
                   this.ObjectId == other.ObjectId &&
                   this.Parameter2 == other.Parameter2 &&
                   this.Parameter3 == other.Parameter3 &&
                   this.MagicPower == other.MagicPower &&
                   this.IdSfxHit == other.IdSfxHit;
        }
    }
}
