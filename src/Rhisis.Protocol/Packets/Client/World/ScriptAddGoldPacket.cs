using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class ScriptAddGoldPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the gold.
    /// </summary>
    public int Gold { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Gold = packet.ReadInt32();
    }
}