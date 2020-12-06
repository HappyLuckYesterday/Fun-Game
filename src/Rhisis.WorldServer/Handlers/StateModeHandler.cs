using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class StateModeHandler
    {
        private readonly ISpecialEffectSystem _specialEffectSystem;

        public StateModeHandler(ISpecialEffectSystem specialEffectSystem)
        {
            _specialEffectSystem = specialEffectSystem;
        }

        [HandlerAction(PacketType.STATEMODE)]
        public void OnStateMode(IPlayer player, StateModePacket packet)
        {
            if (player.StateMode == packet.StateMode)
            {
                if (packet.Flag == StateModeBaseMotion.BASEMOTION_CANCEL)
                {
                    _specialEffectSystem.SetStateModeBaseMotion(player, packet.Flag);
                    player.Delayer.CancelAction(player.Inventory.ItemInUseActionId);
                    player.Inventory.ItemInUseActionId = Guid.Empty;
                }
            }
        }
    }
}
