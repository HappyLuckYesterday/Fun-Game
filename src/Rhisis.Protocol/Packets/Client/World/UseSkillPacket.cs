using System;
using Rhisis.Game.Common;
using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    /// <summary>
    /// Packet structure received from the client when the player uses a skill.
    /// </summary>
    public class UseSkillPacket : IPacketDeserializer
    {
        public ushort Type { get; private set; }

        public ushort SkillIndex { get; private set; }

        public uint TargetObjectId { get; private set; }

        public SkillUseType UseType { get; private set; }

        public bool Control { get; private set; }

        public void Deserialize(IFFPacket packet)
        {
            Type = packet.Read<ushort>();
            SkillIndex = packet.Read<ushort>();
            TargetObjectId = packet.Read<uint>();
            UseType = (SkillUseType)packet.Read<int>();
            Control = Convert.ToBoolean(packet.Read<int>());
        }
    }
}