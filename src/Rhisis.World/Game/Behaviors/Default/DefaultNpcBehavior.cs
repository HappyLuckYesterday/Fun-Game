﻿using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior of a NPC.
    /// </summary>
    [Behavior(BehaviorType.Npc, isDefault: true)]
    public class DefaultNpcBehavior : IBehavior
    {
        private const float OralTextRadius = 50f;

        private readonly INpcEntity _npc;
        private readonly IChatPacketFactory _chatPacketFactory;

        public DefaultNpcBehavior(INpcEntity npcEntity, IChatPacketFactory chatPacketFactory)
        {
            _npc = npcEntity;
            _chatPacketFactory = chatPacketFactory;
        }

        /// <inheritdoc />
        public void Update()
        {
            UpdateOralText();
        }

        /// <inheritdoc />
        public virtual void OnArrived()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void OnTargetKilled(ILivingEntity killedEntity)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void OnKilled(ILivingEntity killerEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update NPC oral text.
        /// </summary>
        private void UpdateOralText()
        {
            if (_npc.NpcData == null)
            {
                return;
            }

            if (_npc.Timers.LastSpeakTime <= Time.TimeInSeconds())
            {
                if (_npc.NpcData.HasDialog && !string.IsNullOrEmpty(_npc.NpcData.Dialog.OralText))
                {
                    IEnumerable<IPlayerEntity> playersArount = from x in _npc.Object.Entities
                                                               where x.Object.Position.IsInCircle(_npc.Object.Position, OralTextRadius) &&
                                                               x is IPlayerEntity
                                                               select x as IPlayerEntity;

                    foreach (IPlayerEntity player in playersArount)
                    {
                        string text = _npc.NpcData.Dialog.OralText.Replace(DialogVariables.PlayerNameText, player.Object.Name);

                        _chatPacketFactory.SendChatTo(_npc, player, text);
                    }

                    _npc.Timers.LastSpeakTime = Time.TimeInSeconds() + RandomHelper.Random(10, 15);
                }
            }
        }
    }
}
