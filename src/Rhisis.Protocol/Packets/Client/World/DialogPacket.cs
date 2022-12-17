using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class DialogPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the dialog owner object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    /// <summary>
    /// Gets the dialog key.
    /// </summary>
    public string DialogKey { get; private set; }

    /// <summary>
    /// Gets the first parameter.
    /// </summary>
    /// <remarks>
    /// Figure out what this value is.
    /// </remarks>
    public int Param { get; private set; }

    /// <summary>
    /// Gets the dialog quest id.
    /// </summary>
    public int QuestId { get; private set; }

    /// <summary>
    /// Gets the quest mover id.
    /// </summary>
    public uint MoverId { get; private set; }

    /// <summary>
    /// Gets the quest player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        ObjectId = packet.ReadUInt32();
        DialogKey = packet.ReadString();
        Param = packet.ReadInt32();
        QuestId = packet.ReadInt32();
        MoverId = packet.ReadUInt32();
        PlayerId = packet.ReadUInt32();
    }
}
