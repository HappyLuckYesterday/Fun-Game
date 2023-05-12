using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client.Taskbar;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.WorldServer.Handlers.Taskbar;

[PacketHandler(PacketType.REMOVEAPPLETTASKBAR)]
internal sealed class RemoveAppletTaskbarHandler : WorldPacketHandler
{
    /// <summary>
    /// Removes an applet from the applet taskbar.
    /// </summary>
    /// <param name="packet">Incoming packet.</param>
    public void Execute(RemoveTaskbarAppletPacket packet)
    {
        Player.Taskbar.Applets.Remove(packet.SlotIndex);
    }
}