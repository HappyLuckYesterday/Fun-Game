﻿using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ScriptRemoveQuestPacket"/> structure.
    /// </summary>
    public struct ScriptRemoveQuestPacket : IEquatable<ScriptRemoveQuestPacket>
    {
        /// <summary>
        /// Gets the gold.
        /// </summary>
        public int QuestId { get; set; }

        /// <summary>
        /// Creates a new <see cref="ScriptRemoveQuestPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ScriptRemoveQuestPacket(INetPacketStream packet)
        {
            this.QuestId = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="ScriptRemoveQuestPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ScriptRemoveQuestPacket"/></param>
        public bool Equals(ScriptRemoveQuestPacket other)
        {
            return this.QuestId == other.QuestId;
        }
    }
}