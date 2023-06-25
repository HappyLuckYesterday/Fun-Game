using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources.Properties;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers;

[PacketHandler(PacketType.REVIVAL_TO_LODESTAR)]
internal sealed class RevivalToLodestarHandler : WorldPacketHandler
{
    public void Execute(RevivalToLodestarPacket _)
    {
        if (!Player.IsDead)
        {
            return;
        }

        Player.Experience.ApplyDeathPenality();
        Player.Health.ApplyDeathRecovery();

        MapRevivalRegionProperties revivalRegion = Player.Map.GetNearestRevivalRegion(Player.Position, false);

        if (revivalRegion is null)
        {
            throw new InvalidOperationException($"Failed to find revival region for map '{Player.Map.Id}'.");
        }

        if (Player.Map.Id != revivalRegion.MapId)
        {
            Map targetRevivalMap = MapManager.Current.Get(revivalRegion.MapId) 
                ?? throw new InvalidOperationException($"Failed to find map with id '{revivalRegion.MapId}'.");

            revivalRegion = targetRevivalMap.GetRevivalRegion(revivalRegion.Key, false)
                ?? throw new InvalidOperationException($"Cannot find revival region for map '{targetRevivalMap.Id}'.");
        }

        // TODO: Remove buffs

        Player.Teleport(revivalRegion.MapId, revivalRegion.RevivalPosition);

        using FFSnapshot snapshots = new(new FFSnapshot[]
        {
            new MotionSnapshot(Player, ObjectMessageType.OBJMSG_ACC_STOP | ObjectMessageType.OBJMSG_STOP_TURN | ObjectMessageType.OBJMSG_STAND),
            new RevivalToLodestarSnapshot(Player)
        });

        Player.SendToVisible(snapshots, sendToSelf: true);
    }
}
