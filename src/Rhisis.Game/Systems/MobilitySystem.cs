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
        public void Execute(IMover movingEntity)
        {
            if (!movingEntity.Spawned)
            {
                return;
            }

            if (movingEntity.DestinationPosition.IsZero() || movingEntity.ObjectState.HasFlag(ObjectState.OBJSTA_STAND))
            {
                return;
            }

            Walk(movingEntity);
        }

        /// <summary>
        /// Process the walk algorithm.
        /// </summary>
        /// <param name="entity">Current entity</param>
        private void Walk(IMover entity)
        {
            float arrivalRange = entity.IsFollowing ? entity.FollowDistance : ArrivalRangeRadius;

            if (entity.Position.IsInCircle(entity.DestinationPosition, arrivalRange))
            {
                entity.ObjectState &= ~ObjectState.OBJSTA_FMOVE;
                entity.ObjectState |= ObjectState.OBJSTA_STAND;
                entity.Position.Copy(entity.DestinationPosition);
                entity.DestinationPosition.Reset();
                entity.Behavior.OnArrived();

                //if (!entity.Battle.IsFighting)
                //{
                //    entity.Object.MovingFlags &= ~ObjectState.OBJSTA_FMOVE;
                //    entity.Object.MovingFlags |= ObjectState.OBJSTA_STAND;
                //    _moverPacketFactory.SendMotion(entity, ObjectMessageType.OBJMSG_STAND);
                //}
            }
            else
            {
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

                entity.Position.X += (float)offsetX;
                entity.Position.Z += (float)offsetZ;
            }
        }
    }
}
