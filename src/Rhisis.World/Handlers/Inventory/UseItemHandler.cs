using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.World.Handlers.Inventory
{
    [Handler]
    public class UseItemHandler
    {
        private readonly ILogger<UseItemHandler> _logger;

        public UseItemHandler(ILogger<UseItemHandler> logger)
        {
            _logger = logger;
        }

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

                // TODO: check if player is fighting
                player.Inventory.Equip(item);
            }
            else
            {
                if (item.Data.IsUseable && item.Quantity > 0)
                {
                    _logger.LogTrace($"{player.Name} want to use {item.Name}.");

                    if (player.Inventory.ItemHasCoolTime(item) && !player.Inventory.CanUseItemWithCoolTime(item))
                    {
                        _logger.LogTrace($"Player '{player.Name}' cannot use item {item.Name}: CoolTime.");
                        return;
                    }

                    // TODO: item usage
                }
            }

            player.Defense.Update();
        }
    }
}
