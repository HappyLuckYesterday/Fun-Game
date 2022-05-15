﻿using Microsoft.Extensions.DependencyInjection;
using Rhisis.Abstractions.Behavior;
using Rhisis.Abstractions.Caching;
using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Features;
using Rhisis.Abstractions.Features.Battle;
using Rhisis.Abstractions.Features.Chat;
using Rhisis.Abstractions.Map;
using Rhisis.Abstractions.Messaging;
using Rhisis.Abstractions.Protocol;
using Rhisis.Abstractions.Systems;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Protocol.Messages.Cluster;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rhisis.Game.Entities
{
    [DebuggerDisplay("{Name} Lv.{Level}")]
    public class Player : IPlayer, IHuman, IMover, IWorldObject
    {
        private readonly Lazy<IFollowSystem> _followSystem;
        private readonly Lazy<ITeleportSystem> _teleportSystem;
        private readonly Lazy<IJobSystem> _jobSystem;
        private readonly Lazy<IPlayerCache> _playerCache;
        private readonly Lazy<IMessaging> _messaging;

        public IGameConnection Connection { get; set; }

        public DateTime LoggedInAt { get; set; }

        public uint Id { get; }

        public int CharacterId { get; set; }

        public WorldObjectType Type { get; set; }

        public ObjectState ObjectState { get; set; }

        public StateFlags ObjectStateFlags { get; set; }

        public StateMode StateMode { get; set; } = StateMode.NONE;

        public int ModelId { get; set; }

        public IMap Map => MapLayer.ParentMap;

        public IMapLayer MapLayer { get; set; }

        public Vector3 Position { get; set; }

        public float Angle { get; set; }

        public short Size { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public int DeathLevel { get; set; }

        public bool Spawned { get; set; }

        public IExperience Experience { get; set; }

        public IGold Gold { get; set; }

        public int Slot { get; set; }

        public AuthorityType Authority { get; set; }

        public ModeType Mode { get; set; }

        public Vector3 DestinationPosition { get; set; } = new Vector3();

        public float Speed
        {
            get
            {
                return (Data.Speed + (Attributes.Get(DefineAttributes.SPEED) / 100)) * SpeedFactor;
            }
        }

        public float SpeedFactor { get; set; } = 1;

        public bool IsMoving => ObjectState.HasFlag(ObjectState.OBJSTA_MOVE_ALL) && !DestinationPosition.IsZero();

        public string CurrentNpcShopName { get; set; }

        public MoverData Data { get; set; }

        public JobData Job { get; set; }

        public IHealth Health { get; set; }

        public IDefense Defense { get; set; }

        public IAttributes Attributes { get; set; }

        public IDelayer Delayer { get; set; }

        public IBuffs Buffs { get; set; }

        IStatistics IMover.Statistics => Statistics;

        public IPlayerStatistics Statistics { get; set; }

        public IInventory Inventory { get; set; }

        public IChat Chat { get; set; }

        public IBattle Battle { get; set; }

        public IQuestDiary Quests { get; set; }

        public ISkillTree SkillTree { get; set; }

        public ITaskbar Taskbar { get; set; }

        public IMessenger Messenger { get; set;  }

        public IProjectiles Projectiles { get; set; }

        public IHumanVisualAppearance Appearence { get; set; }

        public IServiceProvider Systems { get; set; }
        
        public IList<IWorldObject> VisibleObjects { get; set; }

        public IBehavior Behavior { get; set; }

        public IWorldObject FollowTarget { get; set; }

        public float FollowDistance { get; set; }

        public Player()
        {
            Id = RandomHelper.GenerateUniqueId();
            VisibleObjects = new List<IWorldObject>();
            _followSystem = new Lazy<IFollowSystem>(() => Systems.GetService<IFollowSystem>());
            _teleportSystem = new Lazy<ITeleportSystem>(() => Systems.GetService<ITeleportSystem>());
            _jobSystem = new Lazy<IJobSystem>(() => Systems.GetService<IJobSystem>());
            _playerCache = new Lazy<IPlayerCache>(() => Systems.GetService<IPlayerCache>());
            _messaging = new Lazy<IMessaging>(() => Systems.GetService<IMessaging>());
        }

        public void Follow(IWorldObject worldObject) => _followSystem.Value.Follow(this, worldObject);

        public void Unfollow() => _followSystem.Value.Unfollow(this);

        public void Teleport(Vector3 position, bool sendToPlayer = true) => _teleportSystem.Value.Teleport(this, position, sendToPlayer);

        public void Teleport(Vector3 position, int mapId, bool sendToPlayer = true) => _teleportSystem.Value.Teleport(this, position, mapId, sendToPlayer);

        public void ChangeJob(DefineJob.Job newJob) => _jobSystem.Value.ChangeJob(this, newJob);

        public void UpdateCache()
        {
            CachedPlayer player = _playerCache.Value.Get(CharacterId);

            if (player is null)
            {
                throw new InvalidOperationException($"Failed to retrieve cached player information.");
            }

            bool cacheUpdated = false;

            if (player.Level != Level)
            {
                player.Level = Level;
                cacheUpdated = true;
            }

            if (player.Job != Job.Id)
            {
                player.Job = Job.Id;
                cacheUpdated = true;
            }

            if (player.MessengerStatus != Messenger.Status)
            {
                player.MessengerStatus = Messenger.Status;
                cacheUpdated = true;
            }

            if (cacheUpdated)
            {
                _playerCache.Value.Set(player);
                //_messaging.Value.Publish(new PlayerCacheUpdate(CharacterId));
            }
        }

        public void OnConnected()
        {
            if (Messenger.Status != MessengerStatusType.Offline)
            {
                _messaging.Value.SendMessage(new PlayerConnectedMessage
                {
                    Id = CharacterId,
                    Status = Messenger.Status
                });
            }
        }

        public void OnDisconnected()
        {

        }

        public bool Equals(IWorldObject other) => Id == other.Id;

        public void Send(IFFPacket packet) => Connection.Send(packet);

        public void SendToVisible(IFFPacket packet)
        {
            IEnumerable<IPlayer> visiblePlayers = VisibleObjects.OfType<IPlayer>();

            if (visiblePlayers.Any())
            {
                foreach (IPlayer player in visiblePlayers)
                {
                    player.Send(packet);
                }
            }
        }

        public override string ToString() => $"{Name} (Id: {CharacterId})";
    }
}
