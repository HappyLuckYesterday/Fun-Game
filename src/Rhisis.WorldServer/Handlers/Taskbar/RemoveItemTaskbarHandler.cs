using Rhisis.Game;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client.Taskbar;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Taskbar;

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
        TaskbarContainer<Shortcut> taskbarContainer = Player.Taskbar.Items.GetContainerAtLevel(packet.SlotLevelIndex)
            ?? throw new InvalidOperationException($"Failed to remove item from item taskbar level: '{packet.SlotLevelIndex}'.");
        
        taskbarContainer.Remove(packet.SlotIndex);
    }
}