using Rhisis.Abstractions.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.World;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers;

[Handler]
public class StatisticsHandler
{
    /// <summary>
    /// Handles the MODIFY_STATUS for updating a player's statistics.
    /// </summary>
    /// <param name="serverClient">Current client.</param>
    /// <param name="packet">Incoming packet.</param>
    [HandlerAction(PacketType.MODIFY_STATUS)]
    public void OnModifyStatus(IPlayer player, ModifyStatusPacket packet)
    {
        player.Statistics.UpdateStatistics(packet.Strength, packet.Stamina, packet.Dexterity, packet.Intelligence);
    }
}
