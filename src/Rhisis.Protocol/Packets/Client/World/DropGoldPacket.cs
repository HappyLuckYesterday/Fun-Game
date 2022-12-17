using Rhisis.Core.Structures;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class DropGoldPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the amount of gold.
    /// </summary>
    public uint Gold { get; private set; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Gold = packet.ReadUInt32();
        Position = new Vector3(packet.ReadSingle(), packet.ReadSingle(), packet.ReadSingle());
    }
}