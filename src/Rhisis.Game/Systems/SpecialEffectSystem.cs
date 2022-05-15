using Rhisis.Core.DependencyInjection;
using Rhisis.Abstractions;
using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Protocol.Snapshots;

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
