using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class NpcDialogHandler
    {
        /// <summary>
        /// Opens a dialog script.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming <see cref="DialogPacket"/>.</param>
        [HandlerAction(PacketType.SCRIPTDLG)]
        public void OnDialogScript(IPlayer player, DialogPacket packet)
        {
            if (packet.ObjectId <= 0)
            {
                throw new ArgumentException("Invalid object id.");
            }

            INpc npc = player.VisibleObjects.OfType<INpc>().FirstOrDefault(x => x.Id == packet.ObjectId);

            if (npc == null)
            {
                throw new ArgumentException($"Cannot find NPC with object id: {packet.ObjectId}");
            }

            npc.OpenDialog(player, packet.Key, packet.QuestId);
        }
    }
}