using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Entities;
using System;

namespace Rhisis.Game.Behavior.Default
{
    [Behavior(BehaviorType.Player, IsDefault = true)]
    public class PlayerBehavior : IBehavior
    {
        private readonly IPlayer _player;

        public PlayerBehavior(IPlayer player)
        {
            _player = player;
        }

        public void OnArrived()
        {
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
        }
    }
}
