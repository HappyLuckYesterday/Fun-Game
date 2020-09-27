using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.Common.Resources.Quests;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rhisis.Game.Entities
{
    [DebuggerDisplay("{Name} ({Key})")]
    public class Npc : INpc
    {
        private readonly Lazy<IDialogSystem> _dialogSystem;
        private readonly Lazy<IChatSystem> _chatSystem;

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

        public IList<IWorldObject> VisibleObjects { get; set; } = new List<IWorldObject>();

        public string Key { get; set; }

        public NpcData Data { get; set; }

        public IBehavior Behavior { get; set; }

        public IItemContainer[] Shop { get; set; }

        public DialogData Dialog => Data.Dialog;

        public IEnumerable<IQuestScript> Quests { get; set; }

        public Npc()
        {
            Id = RandomHelper.GenerateUniqueId();
            _dialogSystem = new Lazy<IDialogSystem>(() => Systems.GetService<IDialogSystem>());
            _chatSystem = new Lazy<IChatSystem>(() => Systems.GetService<IChatSystem>());
        }

        public void OpenDialog(IPlayer player, string dialogKey, int questId)
        {
            _dialogSystem.Value.OpenNpcDialog(this, player, dialogKey, questId);
        }

        public void Speak(string text) => _chatSystem.Value.Speak(this, text);

        public bool Equals(IWorldObject other) => Id == other.Id;
    }
}
