using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client.Taskbar;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Taskbar;

[PacketHandler(PacketType.ADDITEMTASKBAR)]
internal sealed class AddItemTaskbarHandler : WorldPacketHandler
{
    /// <summary>
    /// Adds an item to the item taskbar.
    /// </summary>
    /// <param name="packet">Incoming packet.</param>
    public void Execute(AddTaskbarItemPacket packet)
    {
        var shortcutItemIndex = packet.Type != ShortcutType.Item && packet.ObjectId == 0 ? null : (int?)packet.ObjectId;
        Shortcut shortcutToAdd = new(
            packet.SlotIndex, 
            packet.Type, 
            shortcutItemIndex, 
            packet.ObjectType, 
            packet.ObjectIndex, 
            packet.UserId, 
            packet.ObjectData, 
            packet.Text);

        TaskbarContainer<Shortcut> taskbarContainer = Player.Taskbar.Items.GetContainerAtLevel(packet.SlotLevelIndex)
            ?? throw new InvalidOperationException($"Failed to add item in item taskbar level: '{packet.SlotLevelIndex}'.");
        
        taskbarContainer.Add(shortcutToAdd, packet.SlotIndex);
    }
}