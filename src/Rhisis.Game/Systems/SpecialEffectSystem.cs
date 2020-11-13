using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public sealed class SpecialEffectSystem : GameFeature, ISpecialEffectSystem
    {
        public void SetStateModeBaseMotion(IWorldObject worldObject, StateModeBaseMotion motionState, IItem item = null)
        {
            if (motionState == StateModeBaseMotion.BASEMOTION_ON)
            {
                worldObject.StateMode |= StateMode.BASEMOTION_MODE;
            }
            else
            {
                worldObject.StateMode &= ~StateMode.BASEMOTION_MODE;
            }

            using var modeSnapshot = new StateModeSnapshot(worldObject, motionState, item);

            SendPacketToVisible(worldObject, modeSnapshot, sendToPlayer: true);
        }

        public void StartSpecialEffect(IWorldObject worldObject, DefineSpecialEffects specialEffect, bool followObject = true)
        {
            using var snapshot = new CreateSfxObjectSnapshot(worldObject, specialEffect, followObject);

            SendPacketToVisible(worldObject, snapshot, sendToPlayer: true);
        }
    }
}
