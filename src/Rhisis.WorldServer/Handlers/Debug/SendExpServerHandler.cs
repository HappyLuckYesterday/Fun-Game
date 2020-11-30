using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers.Debug
{
    /// <summary>
    /// Handles every packets related to the exp server system.
    /// </summary>
    [Handler]
    public sealed class SendExpServerHandler
    {
        /// <summary>
        /// Handles a Send Exp Server request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.SEND_TO_SERVER_EXP)]
        public void Execute(IPlayer player, ExperiencePacket packet)
        {
            if (packet.Experience <= 0)
            {
                throw new ArgumentException($"Incorrect experience.");
            }

            if (player.Authority == AuthorityType.GameMaster || player.Authority == AuthorityType.Administrator)
            {
                player.Experience.Increase(packet.Experience);
            }
            else
            {
                throw new InvalidOperationException($"Player '{player.Name}' tried to give himself experience through debug packet.");
            }
        }
    }
}