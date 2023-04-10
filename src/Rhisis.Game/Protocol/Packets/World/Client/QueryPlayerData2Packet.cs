using Rhisis.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class QueryPlayerData2Packet
{
    /// <summary>
    /// Gets the size of the list.
    /// </summary>
    public uint Size { get; private set; }

    /// <summary>
    /// Gets the player id and version.
    /// </summary>
    public Dictionary<uint, int> PlayerDictionary { get; private set; }

    public QueryPlayerData2Packet(FFPacket packet)
    {
        PlayerDictionary = new Dictionary<uint, int>();
        Size = packet.ReadUInt32();
        for (uint i = 0; i < Size; i++)
            PlayerDictionary.Add(packet.ReadUInt32(), packet.ReadInt32());
    }
}
