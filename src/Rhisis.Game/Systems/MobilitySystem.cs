using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public sealed class MobilitySystem : IMobilitySystem
    {
        private const float ArrivalRangeRadius = 1f;

        /// <inheritdoc />
        public void CalculatePosition(IMover movingEntity)
        {
            if (!movingEntity.Spawned)
            {
                return;
            }

            if (movingEntity.DestinationPosition.IsZero() || movingEntity.ObjectState.HasFlag(ObjectState.OBJSTA_STAND))
            {
                return;
            }

            //if (movingEntity.Follow.IsFollowing)
            //{
            //    if (movingEntity.Object.Position.IsInCircle(movingEntity.Follow.Target.Object.Position, movingEntity.Follow.FollowDistance))
            //    {
            //        movingEntity.Moves.DestinationPosition.Reset();
            //        movingEntity.Object.MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
            //        movingEntity.Object.MovingFlags |= ObjectState.OBJSTA_STAND;
            //        movingEntity.Behavior?.OnArrived();
            //    }
            //    else
            //    {
            //        _followSystem.Follow(movingEntity, movingEntity.Follow.Target, movingEntity.Follow.FollowDistance);
            //    }
            //}

            Walk(movingEntity);
        }

        /// <summary>
        /// Process the walk algorithm.
        /// </summary>
        /// <param name="entity">Current entity</param>
        private void Walk(IMover entity)
        {
            if (entity.Position.IsInCircle(entity.DestinationPosition, ArrivalRangeRadius))
            {
                entity.Position.Copy(entity.DestinationPosition);
                entity.DestinationPosition.Reset();

                Console.WriteLine("Arrived");

                //if (!entity.Battle.IsFighting)
                //{
                //    entity.Object.MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
                //    entity.Object.MovingFlags |= ObjectState.OBJSTA_STAND;
                //    _moverPacketFactory.SendMotion(entity, ObjectMessageType.OBJMSG_STAND);
                //}

                //entity.Behavior.OnArrived();
            }
            else
            {
                Console.WriteLine("Walking");
                entity.Angle = Vector3.AngleBetween(entity.Position, entity.DestinationPosition);
                var entitySpeed = entity.Speed;

                if (entity.ObjectStateFlags.HasFlag(StateFlags.OBJSTAF_WALK))
                {
                    entitySpeed /= 4f;
                }
                else if (entity.ObjectState.HasFlag(ObjectState.OBJSTA_BMOVE))
                {
                    entitySpeed /= 5f;
                }

                float distanceX = entity.DestinationPosition.X - entity.Position.X;
                float distanceZ = entity.DestinationPosition.Z - entity.Position.Z;
                var distance = Math.Sqrt(distanceX * distanceX + distanceZ * distanceZ);

                // Normalize
                var offsetX = distanceX / distance * entitySpeed;
                var offsetZ = distanceZ / distance * entitySpeed;

                if (Math.Abs(offsetX) > Math.Abs(distanceX))
                {
                    offsetX = distanceX;
                }

                if (Math.Abs(offsetZ) > Math.Abs(distanceZ))
                {
                    offsetZ = distanceZ;
                }

                UpdatePosition(entity, (float)offsetX, (float)offsetZ);
            }
        }

        private void UpdatePosition(IMover entity, float x, float z)
        {
            entity.Position.X += x;
            entity.Position.Z += z;
        }
    }
}
