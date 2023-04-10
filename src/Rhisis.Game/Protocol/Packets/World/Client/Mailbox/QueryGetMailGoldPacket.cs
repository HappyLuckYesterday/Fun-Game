using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Mailbox;

public class QueryGetMailGoldPacket
{
    /// <summary>
    /// Gets the id of the mail
    /// </summary>
    public int MailId { get; private set; }

    public QueryGetMailGoldPacket(FFPacket packet)
    {
        MailId = packet.ReadInt32();
    }
}
