using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Handlers.Inventory
{
    [Handler]
    public class MoveItemHandler
    {
        [HandlerAction(PacketType.MOVEITEM)]
        public void OnMoveItem(IPlayer player, MoveItemPacket packet)
        {
        }
    }
}
