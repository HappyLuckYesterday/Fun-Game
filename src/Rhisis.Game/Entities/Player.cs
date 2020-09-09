using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Game;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using System;

namespace Rhisis.Game.Entities
{
    public class Player : IPlayer, IHuman, IMover, IWorldObject
    {
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

        public float Size { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public bool Spawned { get; set; }

        public long Experience { get; set; }

        public int Gold { get; set; }

        public int Slot { get; set; }

        public AuthorityType Authority { get; set; }

        public ModeType Mode { get; set; }

        public Vector3 DestinationPosition { get; set; }

        public float Speed { get; }

        public float SpeedFactor { get; set; }

        public bool IsMoving { get; set; }

        public MoverData Data { get; set; }

        public JobData Job { get; set; }

        public IHealth Health { get; set; }

        public IPlayerStatistics Statistics { get; }

        public VisualAppearenceComponent Appearence { get; set; }

        public IServiceProvider Systems { get; set; }

        public Player()
        {
            Id = RandomHelper.GenerateUniqueId();
            Health = new HealthComponent(this);
            Statistics = new StatisticsComponent(this);
        }
    }
}
