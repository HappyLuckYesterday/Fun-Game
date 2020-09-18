using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
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
            player.ObjectState = ObjectState.OBJSTA_FMOVE;
            player.DestinationPosition = new Vector3(packet.X, packet.Y, packet.Z);

            var destPositionSnapshot = new DestPositionSnapshot(player);

            player.Connection.SendToVisible(destPositionSnapshot);
        }
    }
}
