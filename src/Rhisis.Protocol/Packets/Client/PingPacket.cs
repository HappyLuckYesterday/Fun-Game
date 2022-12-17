using Rhisis.Abstractions.Protocol;
using System;

namespace Rhisis.Protocol.Packets.Client;

/// <summary>
/// Represents the ping packet.
/// </summary>
public class PingPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the ping packet time.
    /// </summary>
    public virtual int Time { get; private set; }

    /// <summary>
    /// Gets a value that indiciates that the ping packet has timeout.
    /// </summary>
    public virtual bool IsTimeOut { get; private set; }

    public void Deserialize(IFFPacket packet)
    {
        try
        {
            Time = packet.ReadInt32();
            IsTimeOut = false;
        }
        catch (Exception)
        {
            Time = 0;
            IsTimeOut = true;
        }
    }
}
