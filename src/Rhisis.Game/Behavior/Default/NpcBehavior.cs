using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Entities;
using System;

namespace Rhisis.Game.Behavior.Default
{
    [Behavior(BehaviorType.Npc, IsDefault = true)]
    public class NpcBehavior : IBehavior
    {
        private readonly INpc _npc;

        public NpcBehavior(INpc npc)
        {
            _npc = npc;
        }

        public void OnArrived()
        {
            throw new NotImplementedException();
        }

        public void OnKilled(IMover killerEntity)
        {
            throw new NotImplementedException();
        }

        public void OnTargetKilled(IMover killedEntity)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            if (!_npc.Spawned)
            {
                return;
            }

            
        }
    }
}
