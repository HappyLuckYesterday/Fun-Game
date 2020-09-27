using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;
using System;

namespace Rhisis.Game.Features
{
    public class Gold : GameFeature, IGold
    {
        private readonly IPlayer _player;

        public int Amount { get; private set; }

        public Gold(IPlayer player, int initialGoldAmount)
        {
            _player = player;
            Amount = initialGoldAmount;
        }

        public bool Decrease(int amount)
        {
            Amount = Math.Max(Amount - amount, 0);

            SendUpdatedGold();

            return true;
        }

        public bool Increase(int amount)
        {
            // We cast player gold to long because otherwise it would use Int32 arithmetic and would overflow
            long gold = (long)Amount + amount;

            if (gold > int.MaxValue || gold < 0) // Check gold overflow
            {
                SendDefinedText(_player, DefineText.TID_GAME_TOOMANYMONEY_USE_PERIN);
                return false;
            }
            else
            {
                Amount = (int)gold;
                SendUpdatedGold();
            }

            return true;
        }

        private void SendUpdatedGold()
        {
            using var goldUpdateSnapshot = new UpdateParamPointSnapshot(_player, DefineAttributes.GOLD, Amount);
            
            _player.Connection.Send(goldUpdateSnapshot);
        }
    }
}
