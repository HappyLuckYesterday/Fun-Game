using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public class VisibilitySystem : IVisibilitySystem
    {
        public void Execute(IMover mover)
        {
            if (!mover.Spawned)
            {
                return;
            }

            if (mover is IPlayer player)
            {
                IEnumerable<IWorldObject> currentVisibleObjects = player.MapLayer.GetVisibleObjects(player);
                IEnumerable<IWorldObject> appearingObjects = currentVisibleObjects.Except(player.VisibleObjects);
                IEnumerable<IWorldObject> disapearingObjects = player.VisibleObjects.Except(currentVisibleObjects);

                if (appearingObjects.Any() || disapearingObjects.Any())
                {
                    var snapshot = new FFSnapshot();

                    foreach (IWorldObject appearingObject in appearingObjects)
                    {
                        snapshot.Merge(new AddObjectSnapshot(appearingObject, AddObjectSnapshot.PlayerAddObjMethodType.ExcludeItems));

                        if (appearingObject is IMover appearingMover && appearingMover.IsMoving)
                        {
                            snapshot.Merge(new DestPositionSnapshot(appearingMover));
                        }

                        if (!appearingObject.VisibleObjects.Contains(player))
                        {
                            appearingObject.VisibleObjects.Add(player);
                        }
                    }

                    foreach (IWorldObject disapearingObject in disapearingObjects)
                    {
                        snapshot.Merge(new DeleteObjectSnapshot(disapearingObject));

                        if (disapearingObject.VisibleObjects.Contains(player))
                        {
                            disapearingObject.VisibleObjects.Remove(player);
                        }
                    }

                    player.Connection.Send(snapshot);
                    player.VisibleObjects = currentVisibleObjects.ToList();
                }
            }
        }
    }
}
