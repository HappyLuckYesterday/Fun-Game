using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Behavior.Default
{
    [Behavior(BehaviorType.Monster, IsDefault = true)]
    public class MonsterBehavior : IBehavior
    {
        private readonly IMonster _monster;

        public MonsterBehavior(IMonster monster)
        {
            _monster = monster;
        }

        public void OnArrived()
        {
            long nextMoveTime = RandomHelper.LongRandom(5, 10);

            _monster.Timers.NextMoveTime = Time.TimeInSeconds() + nextMoveTime;
            //_monster.Object.BeginPosition.Copy(_monster.Object.Position);
        }

        public void OnKilled(IMover killerEntity)
        {
            throw new NotImplementedException();
        }

        public void OnTargetKilled(IMover killedEntity)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            if (!_monster.Spawned || _monster.Health.IsDead)
            {
                return;
            }

            if (_monster.ObjectState.HasFlag(ObjectState.OBJSTA_STAND) && _monster.Timers.NextMoveTime < Time.TimeInSeconds())
            {
                Vector3 randomPosition = _monster.RespawnRegion.GetRandomPosition();

                while (_monster.Position.GetDistance2D(randomPosition) > 10f)
                {
                    randomPosition = _monster.RespawnRegion.GetRandomPosition();
                }

                if (_monster.IsFlying)
                {
                    randomPosition.Y = _monster.Map.GetHeight(randomPosition.X, randomPosition.Z) + RandomHelper.Random(0, 6);
                }

                MoveToPosition(randomPosition);
            }
        }

        private void MoveToPosition(Vector3 destinationPosition)
        {
            _monster.ObjectState &= ~ObjectState.OBJSTA_STAND;
            _monster.ObjectState |= ObjectState.OBJSTA_FMOVE;
            _monster.DestinationPosition.Copy(destinationPosition);
            _monster.Angle = Vector3.AngleBetween(_monster.Position, _monster.DestinationPosition);

            IEnumerable<IPlayer> visiblePlayers = _monster.VisibleObjects.OfType<IPlayer>();

            if (visiblePlayers.Any())
            {
                using var snapshot = new FFSnapshot(new DestPositionSnapshot(_monster), new DestAngleSnapshot(_monster));

                foreach (IPlayer player in visiblePlayers)
                {
                    player.Connection.Send(snapshot);
                }
            }
        }
    }
}
