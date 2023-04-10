using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ScriptRemoveAllItemPacket
{
    /// <summary>
    /// Gets the item id.
    /// </summary>
    public uint ItemId { get; private set; }

    /// <summary>
    /// Creates a new <see cref="ScriptRemoveAllItemPacket"/> object.
    /// </summary>
    /// <param name="packet">Incoming packet</param>
    public ScriptRemoveAllItemPacket(FFPacket packet)
    {
        ItemId = packet.ReadUInt32();
    }
}