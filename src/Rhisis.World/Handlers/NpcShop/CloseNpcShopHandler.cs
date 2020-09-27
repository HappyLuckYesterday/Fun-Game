﻿using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers.NpcShop
{
    [Handler]
    public class CloseNpcShopHandler
    {
        [HandlerAction(PacketType.CLOSESHOPWND)]
        public void Execute(IPlayer player)
        {
            player.CurrentNpcShopName = null;
        }
    }
}
