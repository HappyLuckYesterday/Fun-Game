using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
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
            if (monster.Spawned && monster.Health.IsDead && monster.Timers.DespawnTime < Time.TimeInSeconds())
            {
                monster.Spawned = false;
                monster.Timers.RespawnTime = Time.TimeInSeconds() + monster.RespawnRegion.Time;
            }
            else if (!monster.Spawned && monster.Timers.RespawnTime < Time.TimeInSeconds())
            {
                monster.Position.Copy(monster.RespawnRegion.GetRandomPosition());
                monster.DestinationPosition.Reset();
                monster.Health.RegenerateAll();
                monster.Battle.ClearTarget();
                monster.Unfollow();
                monster.ObjectState = ObjectState.OBJSTA_STAND;
                monster.Timers.NextMoveTime = Time.TimeInSeconds() + RandomHelper.LongRandom(5, 15);
                monster.SpeedFactor = 1;
                monster.Spawned = true;
            }
        }

        private void ProcessMapItemRespawn(IMapItem mapItem)
        {
            if (mapItem.HasOwner && mapItem.OwnershipTime < Time.TimeInSeconds())
            {
                mapItem.Owner = null;
                mapItem.OwnershipTime = 0;
            }

            if (mapItem.IsTemporary && mapItem.DespawnTime < Time.TimeInSeconds())
            {
                mapItem.Spawned = false;
                mapItem.MapLayer.RemoveItem(mapItem);
            }

            if (!mapItem.IsTemporary && !mapItem.Spawned && mapItem.RespawnTime < Time.TimeInSeconds())
            {
                mapItem.Position.Copy(mapItem.RespawnRegion.GetRandomPosition());
                mapItem.Spawned = true;
            }
        }
    }
}
