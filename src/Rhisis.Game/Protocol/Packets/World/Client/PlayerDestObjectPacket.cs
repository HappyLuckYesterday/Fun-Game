using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class PlayerDestObjectPacket
{
    /// <summary>
    /// Gets the target object id.
    /// </summary>
    public uint TargetObjectId { get; private set; }

    /// <summary>
    /// Gets the distance to the target.
    /// </summary>
    public float Distance { get; private set; }

    public PlayerDestObjectPacket(FFPacket packet)
    {
        TargetObjectId = packet.ReadUInt32();
        Distance = packet.ReadSingle();
    }
}