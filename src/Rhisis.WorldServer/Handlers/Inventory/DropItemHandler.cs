using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Factories;
using Rhisis.Game.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Inventory
{
    [Handler]
    public class DropItemHandler
    {
        private readonly IEntityFactory _entityFactory;

        public DropItemHandler(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }

        [HandlerAction(PacketType.DROPITEM)]
        public void Execute(IPlayer player, DropItemPacket packet)
        {
            if (packet.ItemQuantity <= 0)
            {
                throw new InvalidOperationException("Invalid drop quantity.");
            }

            IItem item = player.Inventory.GetItem(packet.ItemUniqueId);

            if (item == null)
            {
                throw new InvalidOperationException($"Cannot find item with index: {packet.ItemUniqueId} in player's inventory.");
            }

            int quantityToDrop = Math.Min(packet.ItemQuantity, item.Quantity);
            IItem itemToDrop = item.Clone();
            itemToDrop.Quantity = quantityToDrop;

            if (player.Inventory.DeleteItem(item, quantityToDrop) > 0)
            {
                IMapItem dropItem = _entityFactory.CreateMapItem(itemToDrop, player.MapLayer, null, player.Position);

                player.MapLayer.AddItem(dropItem);
            }
        }
    }
}
