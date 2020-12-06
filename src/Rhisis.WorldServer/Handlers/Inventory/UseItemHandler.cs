using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Inventory
{
    [Handler]
    public class UseItemHandler
    {
        [HandlerAction(PacketType.DOUSEITEM)]
        public void Execute(IPlayer player, DoUseItemPacket packet)
        {
            IItem item = player.Inventory.GetItem(packet.ItemIndex);

            if (item == null)
            {
                throw new ArgumentException($"Cannot find item with unique id: '{packet.ItemIndex}' in inventory.", nameof(item));
            }

            if (packet.Part > 0)
            {
                if (packet.Part >= (int)ItemPartType.Maximum)
                {
                    throw new InvalidOperationException($"Invalid equipement part.");
                }

                if (!player.Battle.IsFighting)
                {
                    player.Inventory.Equip(item);
                }
            }
            else
            {
                player.Inventory.UseItem(item);
            }

            player.Defense.Update();
        }
    }
}
