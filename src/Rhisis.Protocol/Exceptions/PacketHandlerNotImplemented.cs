using System;

namespace Rhisis.Protocol.Exceptions;

public sealed class PacketHandlerNotImplemented : Exception
{
    public PacketType PacketType { get; }

    public PacketHandlerNotImplemented(PacketType packetType)
		: base($"Packet handler for packet: '{packetType}' is not implemented.")
	{
        PacketType = packetType;
    }
}
