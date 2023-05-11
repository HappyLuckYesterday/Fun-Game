using Rhisis.Game.TaskbarPlayer;
using Rhisis.Game.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Client.Taskbar;

namespace Rhisis.WorldServer.Handlers.PlayerTaskbar;

[PacketHandler(PacketType.ADDITEMTASKBAR)]
internal sealed class AddItemTaskbarHandler : WorldPacketHandler
{
    /// <summary>
    /// Adds an item to the item taskbar.
    /// </summary>
    /// <param name="serverClient">Current client.</param>
    /// <param name="packet">Incoming packet.</param>
    public void Execute(AddTaskbarItemPacket packet)
    {
        var shortcutItemIndex = packet.Type != ShortcutType.Item && packet.ObjectId == 0 ? null : (int?)packet.ObjectId;
        var shortcutToAdd = new Shortcut(packet.SlotIndex, packet.Type, shortcutItemIndex, packet.ObjectType, packet.ObjectIndex, packet.UserId, packet.ObjectData, packet.Text);

        TaskbarContainer<Shortcut> taskbarContainer = Player.Taskbar.Items.GetContainerAtLevel(packet.SlotLevelIndex);

        if (taskbarContainer == null)
        {
            throw new InvalidOperationException($"Failed to add item in item taskbar level: '{packet.SlotLevelIndex}'.");
        }

        taskbarContainer.Add(shortcutToAdd, packet.SlotIndex);
    }
}