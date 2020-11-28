using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class MovementHandler
    {
        /// <summary>
        /// Handles the destination position snapshot.
        /// </summary>
        /// <param name="serverClient">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(SnapshotType.DESTPOS)]
        public void OnSnapshotSetDestPosition(IPlayer player, SetDestPositionPacket packet)
        {
            if (!player.Map.IsInBounds(packet.X, packet.Y, packet.Z))
            {
                throw new InvalidOperationException("Destination position is out of map bounds.");
            }

            player.ObjectState = ObjectState.OBJSTA_FMOVE;
            player.DestinationPosition = new Vector3(packet.X, packet.Y, packet.Z);
            player.Unfollow();
            player.Battle.ClearTarget();
            player.SendToVisible(new DestPositionSnapshot(player));
        }
    }
}
