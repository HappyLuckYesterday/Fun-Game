using Microsoft.Extensions.Options;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Factories;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Entities;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using System.Linq;

namespace Rhisis.Game.Behavior.Default
{
    [Behavior(BehaviorType.Monster, IsDefault = true)]
    public class MonsterBehavior : IBehavior
    {
        private readonly IMonster _monster;
        private readonly IEntityFactory _entityFactory;
        private readonly IGameResources _gameResources;
        private readonly WorldConfiguration _worldServerConfiguration;

        public MonsterBehavior(IMonster monster, IEntityFactory entityFactory, IGameResources gameResources, IOptions<WorldConfiguration> worldServerConfiguration)
        {
            _monster = monster;
            _entityFactory = entityFactory;
            _gameResources = gameResources;
            _worldServerConfiguration = worldServerConfiguration.Value;
        }

        public void OnArrived()
        {
            if (!_monster.Battle.IsFighting)
            {
                long nextMoveTime = _monster.IsReturningToBeginPosition ?
                    RandomHelper.LongRandom(1, 3) :
                    RandomHelper.LongRandom(5, 10);

                _monster.Timers.NextMoveTime = Time.TimeInSeconds() + nextMoveTime;
                _monster.BeginPosition.Copy(_monster.Position);

                if (_monster.IsReturningToBeginPosition)
                {
                    _monster.IsReturningToBeginPosition = false;
                    _monster.Health.RegenerateAll();
                }
                if (_monster.SpeedFactor >= 2)
                {
                    SetSpeedFactor(1);
                }
            }
        }

        public void OnKilled(IMover killerEntity)
        {
            DropGold(killerEntity);
            DropItems(killerEntity);
            _monster.Timers.DespawnTime = Time.TimeInSeconds() + 3;
        }

        public void OnTargetKilled(IMover killedEntity)
        {
            ReturnToBeginPosition();
        }

        public void Update()
        {
            if (!_monster.Spawned || _monster.Health.IsDead)
            {
                return;
            }

            if (_monster.Battle.IsFighting)
            {
                if (_monster.IsFollowing)
                {
                    if (_monster.SpeedFactor != 2)
                    {
                        SetSpeedFactor(2);
                    }

                    if (_monster.Position.IsInCircle(_monster.FollowTarget.Position, _monster.FollowDistance))
                    {
                        if (_monster.Timers.NextAttackTime < Time.TimeInMilliseconds())
                        {
                            _monster.Battle.TryMeleeAttack(_monster.Battle.Target, AttackType.MeleeAttack1);
                            _monster.Timers.NextAttackTime = (long)(Time.TimeInMilliseconds() + _monster.Data.ReAttackDelay);
                        }
                    }
                    else
                    {
                        if (_monster.Position.IsInRange(_monster.BeginPosition, 40f))
                        {
                            _monster.Follow(_monster.Battle.Target);
                        }
                        else
                        {
                            ReturnToBeginPosition();
                        }
                    }
                }
                else
                {
                    _monster.Follow(_monster.Battle.Target);
                }
            }
            else
            {
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

                if (_monster.ObjectState.HasFlag(ObjectState.OBJSTA_FMOVE))
                {
                    if (_monster.IsReturningToBeginPosition)
                    {
                        if (_monster.Position.IsInCircle(_monster.BeginPosition, 3.0f))
                        {
                            SetSpeedFactor(1f);
                        }
                    }
                }
            }
        }

        private void MoveToPosition(Vector3 destinationPosition)
        {
            _monster.ObjectState &= ~ObjectState.OBJSTA_STAND;
            _monster.ObjectState |= ObjectState.OBJSTA_FMOVE;
            _monster.DestinationPosition.Copy(destinationPosition);
            _monster.Angle = Vector3.AngleBetween(_monster.Position, _monster.DestinationPosition);

            using var snapshot = new FFSnapshot(new DestPositionSnapshot(_monster), new DestAngleSnapshot(_monster));
            _monster.SendToVisible(snapshot);
        }

        private void ReturnToBeginPosition()
        {
            _monster.IsReturningToBeginPosition = true;
            _monster.Unfollow();
            _monster.Battle.ClearTarget();
            SetSpeedFactor(2.66f);
            MoveToPosition(_monster.BeginPosition);
        }

        private void DropGold(IMover owner)
        {
            IItem gold = _entityFactory.CreateGoldItem(RandomHelper.Random(_monster.Data.DropGoldMin, _monster.Data.DropGoldMax));
            IMapItem goldItem = _entityFactory.CreateMapItem(gold, _monster.MapLayer, owner, _monster.Position);

            _monster.MapLayer.AddItem(goldItem);
        }

        private void DropItems(IMover owner)
        {
            // TODO: move this constant to configuration file
            const long MaxDropChance = 3000000000;
            
            // Drop items
            int itemCount = 0;
            foreach (DropItemData dropItem in _monster.Data.DropItems)
            {
                if (itemCount >= _monster.Data.MaxDropItem)
                    break;

                long dropChance = RandomHelper.LongRandom(0, MaxDropChance);

                if (dropItem.Probability * _worldServerConfiguration.Rates.Drop >= dropChance)
                {
                    byte itemRefine = (byte)RandomHelper.Random(0, dropItem.ItemMaxRefine);
                    IItem droppedItem = _entityFactory.CreateItem(dropItem.ItemId, itemRefine, ElementType.None, 0);

                    DropItem(droppedItem, owner, _monster.Position);

                    itemCount++;
                }
            }

            // Drop item kinds
            foreach (DropItemKindData dropItemKind in _monster.Data.DropItemsKind)
            {
                var itemsDataByItemKind = _gameResources.Items.Values.Where(x => x.ItemKind3 == dropItemKind.ItemKind && x.Rare >= dropItemKind.UniqueMin && x.Rare <= dropItemKind.UniqueMax);

                if (!itemsDataByItemKind.Any())
                {
                    continue;
                }

                var itemData = itemsDataByItemKind.ElementAt(RandomHelper.Random(0, itemsDataByItemKind.Count() - 1));

                int itemRefine = RandomHelper.Random(0, 10);

                for (int i = itemRefine; i >= 0; i--)
                {
                    long itemDropProbability = (long)(_gameResources.ExpTables.GetDropLuck(itemData.Level > 120 ? 119 : itemData.Level, itemRefine) * (_monster.Data.CorrectionValue / 100f));
                    long dropChance = RandomHelper.LongRandom(0, MaxDropChance);

                    if (dropChance < itemDropProbability * _worldServerConfiguration.Rates.Drop)
                    {
                        IItem itemToDrop = _entityFactory.CreateItem(itemData.Id, (byte)itemRefine, ElementType.None, 0);

                        DropItem(itemToDrop, owner, _monster.Position);

                        break;
                    }
                }
            }
        }

        private void DropItem(IItem item, IMover owner, Vector3 position)
        {
            IMapItem mapItem = _entityFactory.CreateMapItem(item, _monster.MapLayer, owner, position);

            _monster.MapLayer.AddItem(mapItem);
        }

        private void SetSpeedFactor(float speedFactor)
        {
            _monster.SpeedFactor = speedFactor;
            using var speedFactorSnapshot = new SetSpeedFactorSnapshot(_monster, speedFactor);

            _monster.SendToVisible(speedFactorSnapshot);
        }
    }
}
