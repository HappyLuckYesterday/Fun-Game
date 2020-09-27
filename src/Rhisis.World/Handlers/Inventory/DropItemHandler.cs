using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.World.Handlers.Inventory
{
    [Handler]
    public class DropItemHandler
    {
        [HandlerAction(PacketType.DROPITEM)]
        public void Execute(IPlayer player, DropItemPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}
