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
    public class PlayerMovedHandler
    {
        [HandlerAction(PacketType.PLAYERMOVED)]
        public void Execute(IPlayer player, PlayerMovedPacket packet)
        {
            if (player.Health.IsDead)
            {
                throw new InvalidOperationException("Player is dead.");
            }

            // TODO: this handler isn't really correct.
            // We need to review this in order to correct movements.

            player.Unfollow();
            player.Battle.ClearTarget();
            player.DestinationPosition.Reset();
            player.Position.Copy(packet.BeginPosition + packet.DestinationPosition);
            player.Angle = packet.Angle;
            player.ObjectState = (ObjectState)packet.State;
            player.ObjectStateFlags = (StateFlags)packet.StateFlag;

            using var snapshot = new MoverMovedSnapshot(player, 
                packet.BeginPosition, packet.DestinationPosition, player.Angle, 
                (int)player.ObjectState, (int)player.ObjectStateFlags, 
                packet.Motion, packet.MotionEx, packet.Loop, packet.MotionOption, packet.TickCount);

            player.SendToVisible(snapshot);
        }
    }
}
