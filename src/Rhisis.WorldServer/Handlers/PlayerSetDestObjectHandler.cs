using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class PlayerSetDestObjectHandler
    {
        [HandlerAction(PacketType.PLAYERSETDESTOBJ)]
        public void Execute(IPlayer player, PlayerDestObjectPacket packet)
        {
            if (packet.TargetObjectId <= 0)
            {
                throw new InvalidOperationException($"Invalid target object id: '{packet.TargetObjectId}'.");
            }

            if (player.Id == packet.TargetObjectId)
            {
                return;
            }

            IWorldObject targetObject = player.VisibleObjects.Single(x => x.Id == packet.TargetObjectId);

            player.Follow(targetObject);
        }
    }
}
