using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Mailbox;

public class ReadMailPacket
{
    /// <summary>
    /// Gets the id of the mail
    /// </summary>
    public int MailId { get; private set; }

    public ReadMailPacket(FFPacket packet)
    {
        MailId = packet.ReadInt32();
    }
}
