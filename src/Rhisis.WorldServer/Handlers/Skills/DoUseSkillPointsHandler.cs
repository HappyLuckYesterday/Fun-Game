using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Skills;

[PacketHandler(PacketType.DOUSESKILLPOINT)]
internal sealed class DoUseSkillPointsHandler : WorldPacketHandler
{
    public void Execute(DoUseSkillPointsPacket packet)
    {
        if (!packet.Skills.Any())
        {
            return;
        }

        Player.Skills.Update(packet.Skills);
        Player.SendSpecialEffect(DefineSpecialEffects.XI_SYS_EXCHAN01);
    }
}
