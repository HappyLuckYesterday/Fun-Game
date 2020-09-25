using Rhisis.Game.Abstractions.Entities;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Features
{
    public class GameFeature
    {
        protected GameFeature()
        {
        }

        /// <summary>
        /// Sends a packet to every visible player of the given world object.
        /// </summary>
        /// <param name="worldObject">Current world object.</param>
        /// <param name="packet">Packet to be sent.</param>
        /// <param name="sendToPlayer">If true, try to send the packet to the world object if its a <see cref="IPlayer"/>.</param>
        public void SendPacketToVisible(IWorldObject worldObject, INetPacketStream packet, bool sendToPlayer = false)
        {
            IEnumerable<IPlayer> visiblePlayers = worldObject.VisibleObjects.OfType<IPlayer>();

            foreach (IPlayer visiblePlayer in visiblePlayers)
            {
                visiblePlayer.Connection.Send(packet);
            }

            if (worldObject is IPlayer player && sendToPlayer)
            {
                player.Connection.Send(packet);
            }
        }
    }
}
