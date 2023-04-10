using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Mailbox;

public class QueryRemoveMailPacket
{
    /// <summary>
    /// Gets the id of the mail.
    /// </summary>
    public int MailId { get; private set; }

    public QueryRemoveMailPacket(FFPacket packet)
    {
        MailId = packet.ReadInt32();
    }
}
