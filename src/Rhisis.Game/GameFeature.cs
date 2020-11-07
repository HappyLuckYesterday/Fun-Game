using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game
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

        /// <summary>
        /// Sends a defined text message to the given player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="text">Defined text id.</param>
        /// <param name="parameters">Additionnal parameters.</param>
        public void SendDefinedText(IWorldObject worldObject, DefineText text, params object[] parameters)
        {
            if (worldObject is IPlayer player)
            {
                using var definedTextSnapshot = new DefinedTextSnapshot(player, text, parameters);

                player.Connection.Send(definedTextSnapshot);
            }
        }
    }
}
