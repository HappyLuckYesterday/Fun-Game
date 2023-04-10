using Rhisis.Protocol;
using System;

namespace Rhisis.Game.Protocol.Packets;

public sealed class PingPacket
{
    /// <summary>
    /// Gets the ping packet time.
    /// </summary>
    public int Time { get; }

    /// <summary>
    /// Gets a value that indiciates that the ping packet has timeout.
    /// </summary>
    public bool IsTimeOut { get; }

    public PingPacket(FFPacket packet)
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
