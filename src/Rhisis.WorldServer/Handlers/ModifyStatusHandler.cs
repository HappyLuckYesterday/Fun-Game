using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.WorldServer.Handlers;

[PacketHandler(PacketType.MODIFY_STATUS)]
internal sealed class ModifyStatusHandler : WorldPacketHandler
{
    public void Execute(ModifyStatusPacket packet)
    {
        Player.UpdateStatistics(packet.Strength, packet.Stamina, packet.Dexterity, packet.Intelligence);
    }
}