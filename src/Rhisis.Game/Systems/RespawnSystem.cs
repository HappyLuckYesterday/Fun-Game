using Rhisis.Core.DependencyInjection;
using Rhisis.Core.IO;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public class RespawnSystem : IRespawnSystem
    {
        public void Execute(IWorldObject worldObject)
        {
            switch (worldObject)
            {
                case IMonster monster:
                    ProcessMonsterRespawn(monster);
                    break;
                case IMapItem mapItem:
                    ProcessMapItemRespawn(mapItem);
                    break;
                default:
                    break;
            }
        }

        private void ProcessMonsterRespawn(IMonster monster)
        {
        }

        private void ProcessMapItemRespawn(IMapItem mapItem)
        {
            if (mapItem.HasOwner && mapItem.OwnershipTime < Time.TimeInSeconds())
            {
                ResetMapItemOwnership(mapItem);
            }

            if (mapItem.IsTemporary && mapItem.DespawnTime < Time.TimeInSeconds())
            {
                ResetMapItemOwnership(mapItem);
                mapItem.Spawned = false;
                mapItem.MapLayer.RemoveItem(mapItem);
            }

            if (!mapItem.IsTemporary && !mapItem.Spawned && mapItem.RespawnTime <= Time.TimeInSeconds())
            {
                mapItem.Position.Copy(mapItem.RespawnRegion.GetRandomPosition());
                mapItem.Spawned = true;
            }
        }

        private void ResetMapItemOwnership(IMapItem mapItem)
        {
            mapItem.Owner = null;
            mapItem.OwnershipTime = 0;
        }
    }
}
