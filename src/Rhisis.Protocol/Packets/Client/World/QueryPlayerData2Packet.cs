using System.Collections.Generic;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class QueryPlayerData2Packet : IPacketDeserializer
{
    /// <summary>
    /// Gets the size of the list.
    /// </summary>
    public uint Size { get; private set; }

    /// <summary>
    /// Gets the player id and version.
    /// </summary>
    public Dictionary<uint, int> PlayerDictionary { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        PlayerDictionary = new Dictionary<uint, int>();
        Size = packet.ReadUInt32();
        for (uint i = 0; i < Size; i++)
            PlayerDictionary.Add(packet.ReadUInt32(), packet.ReadInt32());
    }
}
