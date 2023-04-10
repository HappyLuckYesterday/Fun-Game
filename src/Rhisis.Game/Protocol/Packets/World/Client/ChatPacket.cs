using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ChatPacket
{
    /// <summary>
    /// Gets the chat message.
    /// </summary>
    public string Message { get; private set; }

    public ChatPacket(FFPacket packet)
    {
        Message = packet.ReadString();
    }
}
