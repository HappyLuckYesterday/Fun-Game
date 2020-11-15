using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using System;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public class TeleportSystem : ITeleportSystem
    {
        private readonly IMapManager _mapManager;

        public TeleportSystem(IMapManager mapManager)
        {
            _mapManager = mapManager;
        }

        public void Teleport(IPlayer player, Vector3 position, bool sendToPlayer = true)
        {
            if (!player.Map.IsInBounds(position))
            {
                throw new InvalidOperationException($"Attempt to teleport '{player.Name}' to an invalid position: {position} in map: '{player.Map.Name}'.");
            }

            SetPlayerPosition(player, position);

            if (sendToPlayer)
            {
                using var snapshots = new FFSnapshot(new SetPositionSnapshot(player), new WorldReadInfoSnapshot(player));

                player.Send(snapshots);
                player.SendToVisible(snapshots);
            }
        }

        public void Teleport(IPlayer player, Vector3 position, int mapId, bool sendToPlayer)
        {
            if (player.Map.Id == mapId)
            {
                Teleport(player, position, sendToPlayer);
                return;
            }

            IMap destinationMap = _mapManager.GetMap(mapId);

            if (destinationMap == null)
            {
                throw new InvalidOperationException($"Cannot teleport to map with id: '{mapId}'. Map not found.");
            }

            if (!destinationMap.IsInBounds(position))
            {
                throw new InvalidOperationException($"Attempt to teleport '{player.Name}' to an invalid position: {position} in map: '{destinationMap.Name}'.");
            }

            player.Spawned = false;
            player.MapLayer.RemovePlayer(player);

            SetPlayerPosition(player, position);

            player.MapLayer = destinationMap.GetDefaultMapLayer();
            player.MapLayer.AddPlayer(player);

            if (sendToPlayer)
            {
                using var snapshots = new FFSnapshot(new ReplaceSnapshot(player), new WorldReadInfoSnapshot(player), new AddObjectSnapshot(player));

                player.Send(snapshots);
            }

            player.Spawned = true;
            player.Projectiles.Reset();
        }

        private void SetPlayerPosition(IPlayer player, Vector3 position)
        {
            player.Unfollow();
            player.Battle.ClearTarget();
            player.DestinationPosition.Reset();

            player.Position.Copy(position);
            player.DestinationPosition.Reset();
            player.ObjectState = ObjectState.OBJSTA_STAND;
        }
    }
}
