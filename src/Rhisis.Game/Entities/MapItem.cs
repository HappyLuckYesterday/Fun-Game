using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Common;
using Sylver.Network.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rhisis.Game.Entities
{
    [DebuggerDisplay("MapItem {Name} ({Map.Name}/{Map.Layer.Id}")]
    public class MapItem : IMapItem
    {
        public uint Id { get; }

        public WorldObjectType Type => WorldObjectType.Item;

        public int ModelId => Item.Data.Id;

        public IMap Map => MapLayer.ParentMap;

        public IMapLayer MapLayer { get; set; }

        public Vector3 Position { get; set; }

        public float Angle { get; set; }

        public short Size { get; set; } = GameConstants.DefaultObjectSize;

        public string Name => Item.Name;

        public IServiceProvider Systems { get; set; }

        public bool Spawned { get; set; }

        public ObjectState ObjectState { get; set; }

        public StateFlags ObjectStateFlags { get; set; }

        public IList<IWorldObject> VisibleObjects { get; set; }

        public IItem Item { get; }

        public MapItemType ItemType { get; }

        public IWorldObject Owner { get; set; }

        public long OwnershipTime { get; set; }

        public long DespawnTime { get; set; }

        public long RespawnTime { get; set; }

        public bool HasOwner => Owner != null && OwnershipTime > 0;

        public bool IsTemporary => DespawnTime > 0;

        public bool IsGold => Item?.Id == DefineItem.II_GOLD_SEED1 ||
            Item?.Id == DefineItem.II_GOLD_SEED2 ||
            Item?.Id == DefineItem.II_GOLD_SEED3 ||
            Item?.Id == DefineItem.II_GOLD_SEED4;

        public IMapRespawnRegion RespawnRegion { get; set; }

        public MapItem(IItem item, MapItemType itemType, IMapLayer mapLayer)
        {
            Id = RandomHelper.GenerateUniqueId();
            Item = item;
            ItemType = itemType;
            MapLayer = mapLayer;
            VisibleObjects = new List<IWorldObject>();
        }

        public bool Equals(IWorldObject other) => Id == other.Id;


        public void Send(INetPacketStream packet)
        {
            throw new InvalidOperationException($"Cannot send a packet to a map item.");
        }

        public void SendToVisible(INetPacketStream packet)
        {
            throw new NotImplementedException();
        }
    }
}
