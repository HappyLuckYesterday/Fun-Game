using Rhisis.Game.TaskbarPlayer;
using Rhisis.Game.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using Rhisis.Game.Protocol.Packets.World.Client.Taskbar;

namespace Rhisis.WorldServer.Handlers.PlayerTaskbar;

[PacketHandler(PacketType.REMOVEITEMTASKBAR)]
internal sealed class RemoveItemTaskbarHandler : WorldPacketHandler
{
    /// <summary>
    /// Removes an item from the item taskbar.
    /// </summary>
    /// <param name="serverClient">Current client.</param>
    /// <param name="packet">Incoming packet.</param>
    public void Execute(RemoveTaskbarItemPacket packet)
    {
        TaskbarContainer<Shortcut> taskbarContainer = Player.Taskbar.Items.GetContainerAtLevel(packet.SlotLevelIndex);

        if (taskbarContainer == null)
        {
            throw new InvalidOperationException($"Failed to remove item from item taskbar level: '{packet.SlotLevelIndex}'.");
        }

        taskbarContainer.Remove(packet.SlotIndex);
    }
}