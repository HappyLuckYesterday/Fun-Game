using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Network.Snapshots;

namespace Rhisis.Game.Behavior.Default
{
    [Behavior(BehaviorType.Player, IsDefault = true)]
    public class PlayerBehavior : IBehavior
    {
        private readonly IPlayer _player;
        private readonly WorldConfiguration _worldServerConfiguration;

        public PlayerBehavior(IPlayer player, IOptions<WorldConfiguration> worldServerConfiguration)
        {
            _player = player;
            _worldServerConfiguration = worldServerConfiguration.Value;
        }

        public void OnArrived()
        {
            if (_player.IsFollowing && _player.FollowTarget is IMapItem mapItem)
            {
                PickupItem(mapItem);
                _player.Unfollow();
            }
        }

        public void OnKilled(IMover killerEntity)
        {
        }

        public void OnTargetKilled(IMover killedEntity)
        {
            if (killedEntity is IMonster deadMonster)
            {
                _player.Experience.Increase(deadMonster.Data.Experience * _worldServerConfiguration.Rates.Experience);
                // TODO: update player quest diary
            }
        }

        public void Update()
        {
            if (_player.Health.IsDead || !_player.Spawned)
            {
                return;
            }

            if (!_player.Battle.IsFighting)
            {
                _player.Health.IdleHeal();
            }
        }

        private void PickupItem(IMapItem mapItem)
        {
            if (mapItem.HasOwner && mapItem.Owner != _player)
            {
                using var defineTextSnapshot = new DefinedTextSnapshot(_player, DefineText.TID_GAME_PRIORITYITEMPER, $"\"{mapItem.Item.Name}\"");
                _player.Send(defineTextSnapshot);

                return;
            }

            if (mapItem.IsGold)
            {
                int goldAmount = mapItem.Item.Quantity;

                if (_player.Gold.Increase(goldAmount))
                {
                    using var defineTextSnapshot = new DefinedTextSnapshot(_player, DefineText.TID_GAME_REAPMONEY, goldAmount.ToString("###,###,###,###"), _player.Gold.Amount.ToString("###,###,###,###"));
                    _player.Send(defineTextSnapshot);
                }
            }
            else
            {
                if (_player.Inventory.CreateItem(mapItem.Item, mapItem.Item.Quantity) > 0)
                {
                    using var defineTextSnapshot = new DefinedTextSnapshot(_player, DefineText.TID_GAME_REAPITEM, $"\"{mapItem.Item.Name}\"");
                    _player.Send(defineTextSnapshot);
                }
            }

            using var motionSnapshot = new MotionSnapshot(_player, ObjectMessageType.OBJMSG_PICKUP);
            _player.SendToVisible(motionSnapshot);
            _player.Send(motionSnapshot);

            _player.MapLayer.RemoveItem(mapItem);
        }
    }
}
