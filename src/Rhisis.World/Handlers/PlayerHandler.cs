using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Interaction;
using Sylver.Network.Data;
using System;

namespace Rhisis.World.Handlers
{

    public sealed class PlayerHandler
    {
        private readonly ILogger<PlayerHandler> _logger;
        private readonly IInteractionSystem _interationSystem;
        private readonly IMoverPacketFactory _moverPacketFactory;

        public PlayerHandler(ILogger<PlayerHandler> logger, IInteractionSystem interationSystem, IMoverPacketFactory moverPacketFactory)
        {
            _logger = logger;
            //_specialEffectSystem = specialEffectSystem;
            _interationSystem = interationSystem;
            _moverPacketFactory = moverPacketFactory;
        }

        //[HandlerAction(PacketType.SETTARGET)]
        public void OnSetTarget(IWorldServerClient serverClient, SetTargetPacket packet)
        {
            _interationSystem.SetTarget(serverClient.Player, packet.TargetId, packet.TargetMode);
        }

        //[HandlerAction(PacketType.QUERY_PLAYER_DATA)]
        public void OnQueryPlayerData(IWorldServerClient serverClient, INetPacketStream packet)
        {
        }

        //[HandlerAction(PacketType.QUERY_PLAYER_DATA2)]
        public void OnQueryPlayerData2(IWorldServerClient serverClient, INetPacketStream packet)
        {
        }

        //[HandlerAction(PacketType.PLAYERMOVED)]
        public void OnPlayerMoved(IWorldServerClient serverClient, PlayerMovedPacket packet)
        {
            if (serverClient.Player.IsDead)
            {
                _logger.LogError($"Player {serverClient.Player.Object.Name} is dead, he cannot move with keyboard.");
                return;
            }

            // TODO: Check if player is flying

            serverClient.Player.Follow.Reset();
            serverClient.Player.Battle.Reset(); 
            serverClient.Player.Moves.DestinationPosition.Reset();
            serverClient.Player.Object.Position = packet.BeginPosition + packet.DestinationPosition;
            serverClient.Player.Object.Angle = packet.Angle;
            serverClient.Player.Object.MovingFlags = (ObjectState)packet.State;
            serverClient.Player.Object.MotionFlags = (StateFlags)packet.StateFlag;

            _moverPacketFactory.SendMoverMoved(serverClient.Player,
                packet.BeginPosition,
                packet.DestinationPosition,
                serverClient.Player.Object.Angle, 
                (int)serverClient.Player.Object.MovingFlags, 
                (int)serverClient.Player.Object.MotionFlags,
                packet.Motion,
                packet.MotionEx,
                packet.Loop,
                packet.MotionOption,
                packet.TickCount);
        }

        //[HandlerAction(PacketType.PLAYERBEHAVIOR)]
        public void OnPlayerBehavior(IWorldServerClient serverClient, PlayerBehaviorPacket packet)
        {
            if (serverClient.Player.IsDead)
            {
                _logger.LogError($"Player {serverClient.Player.Object.Name} is dead, he cannot move with keyboard.");
                return;
            }

            // TODO: check if player is flying

            serverClient.Player.Moves.DestinationPosition.Reset();
            serverClient.Player.Object.Position = packet.BeginPosition + packet.DestinationPosition;
            serverClient.Player.Object.Angle = packet.Angle;
            serverClient.Player.Object.MovingFlags = (ObjectState)packet.State;
            serverClient.Player.Object.MotionFlags = (StateFlags)packet.StateFlag;

            _moverPacketFactory.SendMoverBehavior(serverClient.Player,
                packet.BeginPosition,
                packet.DestinationPosition,
                serverClient.Player.Object.Angle,
                (uint)serverClient.Player.Object.MovingFlags,
                (uint)serverClient.Player.Object.MotionFlags,
                packet.Motion,
                packet.MotionEx,
                packet.Loop,
                packet.MotionOption,
                packet.TickCount);
        }


    }
}
