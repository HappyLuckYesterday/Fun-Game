using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Mailbox;

public class QueryGetMailItemPacket
{
    /// <summary>
    /// Gets the id of the mail
    /// </summary>
    public int MailId { get; private set; }

    public QueryGetMailItemPacket(FFPacket packet)
    {
        MailId = packet.ReadInt32();
    }
}
