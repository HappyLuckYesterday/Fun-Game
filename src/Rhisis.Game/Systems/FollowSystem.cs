using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;
using System;
using System.Linq;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public sealed class FollowSystem : GameSystem, IFollowSystem
    {
        /// <inheritdoc />
        public void Follow(IMover moverEntity, IWorldObject targetMoverEntity, float distance = 1f)
        {
            moverEntity.FollowTarget = targetMoverEntity;
            moverEntity.FollowDistance = distance;
            moverEntity.DestinationPosition.Copy(targetMoverEntity.Position);
            moverEntity.ObjectState &= ~ObjectState.OBJSTA_STAND;
            moverEntity.ObjectState |= ObjectState.OBJSTA_FMOVE;

            using var snapshot = new MoverSetDestObjectSnapshot(moverEntity, targetMoverEntity, distance);
            SendPacketToVisible(moverEntity, snapshot);
        }

        /// <inheritdoc />
        public void Follow(IMover moverEntity, uint targetId, float distance = 1f)
        {
            var entityToFollow = moverEntity.VisibleObjects.FirstOrDefault(x => x.Id == targetId);

            if (entityToFollow == null)
            {
                throw new ArgumentException($"Cannot find entity with object id: {targetId} around {moverEntity.Name}", nameof(entityToFollow));
            }

            Follow(moverEntity, entityToFollow, distance);
        }

        public void Unfollow(IMover moverEntity)
        {
            moverEntity.FollowTarget = null;
            moverEntity.FollowDistance = 0;
        }
    }
}
