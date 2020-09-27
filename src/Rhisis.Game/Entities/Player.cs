using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Features;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rhisis.Game.Entities
{
    [DebuggerDisplay("{Name} Lv.{Level}")]
    public class Player : IPlayer, IHuman, IMover, IWorldObject
    {
        private readonly Lazy<IChatSystem> _chatSystem;
        private readonly Lazy<IFollowSystem> _followSystem;

        public IGameConnection Connection { get; set; }

        public uint Id { get; }

        public int CharacterId { get; set; }

        public WorldObjectType Type { get; set; }

        public ObjectState ObjectState { get; set; }

        public StateFlags ObjectStateFlags { get; set; }

        public int ModelId { get; set; }

        public IMap Map { get; set; }

        public IMapLayer MapLayer { get; set; }

        public Vector3 Position { get; set; }

        public float Angle { get; set; }

        public short Size { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public bool Spawned { get; set; }

        public long Experience { get; set; }

        public IGold Gold { get; set; }

        public int Slot { get; set; }

        public AuthorityType Authority { get; set; }

        public ModeType Mode { get; set; }

        public Vector3 DestinationPosition { get; set; } = new Vector3();

        public float Speed
        {
            get
            {
                // TODO: add attribute speed
                return Data.Speed * SpeedFactor;
            }
        }

        public float SpeedFactor { get; set; } = 1;

        public bool IsMoving => ObjectState.HasFlag(ObjectState.OBJSTA_MOVE_ALL) && !DestinationPosition.IsZero();

        public string CurrentNpcShopName { get; set; }

        public MoverData Data { get; set; }

        public JobData Job { get; set; }

        public IHealth Health { get; set; }

        public IAttributes Attributes { get; set; }

        public IPlayerStatistics Statistics { get; }

        public IInventory Inventory { get; }

        public IHumanVisualAppearance Appearence { get; set; }

        public IServiceProvider Systems { get; set; }
        
        public IList<IWorldObject> VisibleObjects { get; set; }

        public IBehavior Behavior { get; set; }

        public IWorldObject FollowTarget { get; set; }

        public float FollowDistance { get; set; }

        public Player()
        {
            Id = RandomHelper.GenerateUniqueId();
            Health = new Health(this);
            Attributes = new Attributes(this);
            Statistics = new PlayerStatisticsComponent(this);
            Inventory = new Inventory(this);
            VisibleObjects = new List<IWorldObject>();
            _chatSystem = new Lazy<IChatSystem>(() => Systems.GetService<IChatSystem>());
            _followSystem = new Lazy<IFollowSystem>(() => Systems.GetService<IFollowSystem>());
        }

        public void Speak(string text) => _chatSystem.Value.Speak(this, text);

        public void Shout(string text) => _chatSystem.Value.Shout(this, text);

        public void Follow(IWorldObject worldObject) => _followSystem.Value.Follow(this, worldObject);

        public void Unfollow() => _followSystem.Value.Unfollow(this);

        public bool Equals(IWorldObject other) => Id == other.Id;
    }
}
