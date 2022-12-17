using Rhisis.Core.Structures;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class TeleSkillPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the position
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Position = new Vector3(packet.ReadSingle(), packet.ReadSingle(), packet.ReadSingle());
    }
}