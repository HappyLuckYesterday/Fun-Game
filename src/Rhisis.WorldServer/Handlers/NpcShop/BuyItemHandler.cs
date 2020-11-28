using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.NpcShop
{
    [Handler]
    public class BuyItemHandler
    {
        [HandlerAction(PacketType.BUYITEM)]
        public void Execute(IPlayer player, BuyItemPacket packet)
        {
            INpc npc = player.VisibleObjects.OfType<INpc>().SingleOrDefault(x => x.Name == player.CurrentNpcShopName);

            if (npc == null)
            {
                throw new ArgumentException($"Cannot find NPC with name: {player.CurrentNpcShopName}");
            }

            if (!npc.HasShop)
            {
                throw new InvalidOperationException($"NPC '{npc.Name}' doesn't have a shop.");
            }

            if (packet.Tab < 0 || packet.Tab >= npc.Shop.Count())
            {
                throw new InvalidOperationException($"Attempt to buy an item from {npc.Name} shop tab that is out of range.");
            }

            IItemContainer shopTab = npc.Shop.ElementAt(packet.Tab);

            if (packet.Slot < 0 || packet.Slot > shopTab.MaxCapacity - 1)
            {
                throw new InvalidOperationException($"Item slot index was out of tab bounds. Slot: {packet.Slot}");
            }

            IItem shopItem = shopTab.GetItemAtSlot(packet.Slot);

            if (shopItem.Id != packet.ItemId)
            {
                throw new InvalidOperationException($"Shop item id doens't match the item id that {player.Name} is trying to buy.");
            }

            if (player.Gold.Amount < shopItem.Data.Cost)
            {
                using (var defineTextSnapshot = new DefinedTextSnapshot(player, DefineText.TID_GAME_LACKMONEY))
                {
                    player.Connection.Send(defineTextSnapshot);
                }

                throw new InvalidOperationException($"{player.Name} doens't have enough gold to buy item {shopItem.Data.Name} at {shopItem.Data.Cost}.");
            }

            int createdItemsAmount = player.Inventory.CreateItem(shopItem, packet.Quantity);

            if (createdItemsAmount > 0)
            {
                player.Gold.Decrease(shopItem.Data.Cost * createdItemsAmount);
            }
        }
    }
}
