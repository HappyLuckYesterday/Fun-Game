using Rhisis.Game;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Taskbar;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers.PlayerTaskbar
{
    [Handler]
    public class AddAppletTaskbarHandler
    {
        /// <summary>
        /// Adds a new applet to the applet taskbar.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.ADDAPPLETTASKBAR)]
        public void Execute(IPlayer player, AddTaskbarAppletPacket packet)
        {
            var shortcutItemIndex = packet.Type != Core.Common.ShortcutType.Item && packet.ObjectId == 0 ? null : (int?)packet.ObjectId;
            var shortcutToAdd = new Shortcut(packet.SlotIndex, packet.Type, shortcutItemIndex, packet.ObjectType, packet.ObjectIndex, packet.UserId, packet.ObjectData, packet.Text);

            player.Taskbar.Applets.Add(shortcutToAdd, shortcutToAdd.Slot);
        }
    }
}