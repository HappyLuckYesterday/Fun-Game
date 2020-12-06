using Rhisis.Game;
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
    public class AddItemTaskbarHandler
    {
        /// <summary>
        /// Adds an item to the item taskbar.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.ADDITEMTASKBAR)]
        public void Execute(IPlayer player, AddTaskbarItemPacket packet)
        {
            var shortcutItemIndex = packet.Type != Core.Common.ShortcutType.Item && packet.ObjectId == 0 ? null : (int?)packet.ObjectId;
            var shortcutToAdd = new Shortcut(packet.SlotIndex, packet.Type, shortcutItemIndex, packet.ObjectType, packet.ObjectIndex, packet.UserId, packet.ObjectData, packet.Text);

            ITaskbarContainer<IShortcut> taskbarContainer = player.Taskbar.Items.GetContainerAtLevel(packet.SlotLevelIndex);

            if (taskbarContainer == null)
            {
                throw new InvalidOperationException($"Failed to add item in item taskbar level: '{packet.SlotLevelIndex}'.");
            }

            taskbarContainer.Add(shortcutToAdd, packet.SlotIndex);
        }
    }
}