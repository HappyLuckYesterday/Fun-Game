using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World.Taskbar;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers.PlayerTaskbar
{
    [Handler]
    public class RemoveAppletTaskbarHandler
    {
        /// <summary>
        /// Removes an applet from the applet taskbar.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.REMOVEAPPLETTASKBAR)]
        public void Execute(IPlayer player, RemoveTaskbarAppletPacket packet)
        {
            player.Taskbar.Applets.Remove(packet.SlotIndex);
        }
    }
}