using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class SnapshotPacket
{
    public int Count { get; }

    public byte[] Data { get; }

    public SnapshotPacket(FFPacket packet)
    {
        Count = packet.ReadByte();
        Data = packet.ReadBytes((int)(packet.Length - packet.Position));
    }
}
