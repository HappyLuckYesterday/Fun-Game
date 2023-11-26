using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.WorldServer.Handlers;

[PacketHandler(PacketType.MOTION)]
internal sealed class MotionHandler : WorldPacketHandler
{
    public void Execute(MotionPacket packet)
    {
        Player.SendMotion(packet.MotionEnum);
    }
}

