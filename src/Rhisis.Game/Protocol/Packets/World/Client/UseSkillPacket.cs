using Rhisis.Game;
using Rhisis.Protocol;
using System;

namespace Rhisis.Game.Protocol.Packets.World.Client;

/// <summary>
/// Packet structure received from the client when the player uses a skill.
/// </summary>
public class UseSkillPacket
{
    public ushort Type { get; private set; }

    public ushort SkillIndex { get; private set; }

    public uint TargetObjectId { get; private set; }

    public SkillUseType UseType { get; private set; }

    public bool Control { get; private set; }

    public UseSkillPacket(FFPacket packet)
    {
        Type = packet.ReadUInt16();
        SkillIndex = packet.ReadUInt16();
        TargetObjectId = packet.ReadUInt32();
        UseType = (SkillUseType)packet.ReadInt32();
        Control = Convert.ToBoolean(packet.ReadInt32());
    }
}