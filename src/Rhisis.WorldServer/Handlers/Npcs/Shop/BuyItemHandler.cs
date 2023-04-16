using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Npcs.Shop;

[PacketHandler(PacketType.BUYITEM)]
internal sealed class BuyItemHandler : WorldPacketHandler
{
    public void Execute(BuyItemPacket packet)
    {
        Npc npc = Player.VisibleObjects.OfType<Npc>().SingleOrDefault(x => x.Name == Player.CurrentShopName)
            ?? throw new ArgumentException($"Cannot find NPC with name: {Player.CurrentShopName}");

        if (!npc.HasShop)
        {
            throw new InvalidOperationException($"NPC '{npc.Name}' doesn't have a shop.");
        }

        if (packet.Tab < 0 || packet.Tab >= npc.Shop.Length)
        {
            throw new InvalidOperationException($"Attempt to buy an item from {npc.Name} shop tab that is out of range.");
        }

        ItemContainer shopTab = npc.Shop[packet.Tab] ?? throw new InvalidOperationException($"Invalid shop tab '{packet.Tab}'.");

        if (packet.Slot < 0 || packet.Slot > shopTab.MaxCapacity - 1)
        {
            throw new InvalidOperationException($"Item slot index was out of tab bounds. Slot: {packet.Slot}");
        }

        ItemContainerSlot shopItemSlot = shopTab.GetAtSlot(packet.Slot);

        if (!shopItemSlot.HasItem)
        {
            throw new InvalidOperationException($"Item slot at '{packet.Slot}' doesn't contain any item.");
        }

        if (shopItemSlot.Item.Id != packet.ItemId)
        {
            throw new InvalidOperationException($"Shop item id doens't match the item id that {Player.Name} is trying to buy.");
        }

        if (Player.Gold.Amount < shopItemSlot.Item.Properties.Cost)
        {
            Player.SendDefinedText(DefineText.TID_GAME_LACKMONEY);

            throw new InvalidOperationException($"{Player.Name} doens't have enough gold to buy item {shopItemSlot.Item.Name} at {shopItemSlot.Item.Properties.Cost}.");
        }

        int createdItemsAmount = Player.Inventory.CreateItem(new Item(shopItemSlot.Item.Properties)
        {
            Refine = shopItemSlot.Item.Refine,
            Element = shopItemSlot.Item.Element,
            ElementRefine = shopItemSlot.Item.ElementRefine,
            Quantity = packet.Quantity
        });

        if (createdItemsAmount > 0)
        {
            Player.Gold.Decrease(shopItemSlot.Item.Properties.Cost * createdItemsAmount);
        }
    }
}
