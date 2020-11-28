using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Taskbar;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.PlayerTaskbar
{
    [Handler]
    public class RemoveItemTaskbarHandler
    {
        /// <summary>
        /// Removes an item from the item taskbar.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.REMOVEITEMTASKBAR)]
        public void Execute(IPlayer player, RemoveTaskbarItemPacket packet)
        {
            ITaskbarContainer<IShortcut> taskbarContainer = player.Taskbar.Items.GetContainerAtLevel(packet.SlotLevelIndex);

            if (taskbarContainer == null)
            {
                throw new InvalidOperationException($"Failed to remove item from item taskbar level: '{packet.SlotLevelIndex}'.");
            }

            taskbarContainer.Remove(packet.SlotIndex);
        }
    }
}