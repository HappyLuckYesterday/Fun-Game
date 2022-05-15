﻿using Rhisis.Core.DependencyInjection.Extensions;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Abstractions.Behavior;
using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Features;
using Rhisis.Abstractions.Features.Chat;
using Rhisis.Abstractions.Map;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.Common.Resources.Quests;
using Rhisis.Game.Features.Chat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Game.Entities
{
    [DebuggerDisplay("{Name} ({Key})")]
    public class Npc : INpc
    {
        public uint Id { get; }

        public WorldObjectType Type => WorldObjectType.Mover;

        public int ModelId { get; set; }

        public IMap Map { get; set; }

        public IMapLayer MapLayer { get; set; }

        public Vector3 Position { get; set; }

        public float Angle { get; set; }

        public short Size { get; set; }

        public string Name => Key;

        public IServiceProvider Systems { get; set; }

        public bool Spawned { get; set; }
        
        public ObjectState ObjectState { get; set; }
        
        public StateFlags ObjectStateFlags { get; set; }

        public StateMode StateMode { get; set; }

        public IList<IWorldObject> VisibleObjects { get; set; } = new List<IWorldObject>();

        public string Key { get; set; }

        public NpcData Data { get; set; }

        public IBehavior Behavior { get; set; }

        public IItemContainer[] Shop { get; set; }

        public DialogData Dialog => Data.Dialog;

        public IEnumerable<IQuestScript> Quests { get; set; }

        public IChat Chat { get; set; }

        public Npc()
        {
            Id = RandomHelper.GenerateUniqueId();
            Chat = Systems.CreateInstance<Chat>(this);
        }

        public bool Equals(IWorldObject other) => Id == other.Id;

        public void Send(IFFPacket packet) => throw new InvalidOperationException("A NPC cannot send a packet to itself.");

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

        public override string ToString() => $"{Name}";
    }
}
