using LiteNetwork.Server;
using System.IO;
using System.Text.Json;

namespace Rhisis.Protocol;

public class FFInterServerConnection : LiteServerUser
{
    public void Send(CorePacketType packet, object message = null)
    {
        using BinaryWriter writer = new(new MemoryStream());
        writer.Write((byte)packet);

        if (message is not null)
        {
            writer.Write(JsonSerializer.Serialize(message));
        }

        Send(writer.BaseStream);
    }

    public void Disconnect()
    {
        Dispose();
    }
}
