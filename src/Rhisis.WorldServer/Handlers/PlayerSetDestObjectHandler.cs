using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers;

[PacketHandler(PacketType.PLAYERSETDESTOBJ)]
internal sealed class PlayerSetDestObjectHandler : WorldPacketHandler
{
    public void Execute(PlayerDestObjectPacket packet)
    {
        if (packet.TargetObjectId <= 0)
        {
            throw new InvalidOperationException($"Invalid target object id: '{packet.TargetObjectId}'.");
        }

        if (Player.ObjectId == packet.TargetObjectId)
        {
            return;
        }

        WorldObject worldObject = Player.VisibleObjects.SingleOrDefault(x => x.ObjectId == packet.TargetObjectId) 
            ?? throw new InvalidOperationException($"Cannot find object with id: {packet.TargetObjectId} in '{Player.Name}' visible objects.");
        
        Player.Follow(worldObject);
    }
}