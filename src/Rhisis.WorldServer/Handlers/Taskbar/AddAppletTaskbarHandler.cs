using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client.Taskbar;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.WorldServer.Handlers.Taskbar;

[PacketHandler(PacketType.ADDAPPLETTASKBAR)]
internal sealed class AddAppletTaskbarHandler : WorldPacketHandler
{
    /// <summary>
    /// Adds a new applet to the applet taskbar.
    /// </summary>
    /// <param name="packet">Incoming packet.</param>
    public void Execute(AddTaskbarAppletPacket packet)
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

        Player.Taskbar.Applets.Add(shortcutToAdd, shortcutToAdd.Slot);
    }
}