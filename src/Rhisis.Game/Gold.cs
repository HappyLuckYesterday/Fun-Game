using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using System;

namespace Rhisis.Game;

public sealed class Gold
{
    private readonly Player _player;

    /// <summary>
    /// Gets the gold amount.
    /// </summary>
    public int Amount { get; private set; }

    public Gold(Player player)
    {
        _player = player;
    }

    /// <summary>
    /// Initialize the gold component.
    /// </summary>
    /// <param name="initialGoldAmount">Initial gold amount.</param>
    public void Initialize(int initialGoldAmount) => Amount = initialGoldAmount;

    /// <summary>
    /// Decrease the gold amount.
    /// </summary>
    /// <param name="amount">Amount of gold to remove.</param>
    /// <returns>True if the gold amount has been decreased; false otherwise.</returns>
    public bool Decrease(int amount)
    {
        Amount = Math.Max(Amount - amount, 0);

        SendUpdatedGold();

        return true;
    }

    /// <summary>
    /// Increases the gold amount.
    /// </summary>
    /// <param name="amount">Amount of gold to add.</param>
    /// <returns>True if the gold amount has been increased; false otherwise.</returns>
    public bool Increase(int amount)
    {
        // We cast player gold to long because otherwise it would use Int32 arithmetic and would overflow
        long gold = (long)Amount + amount;

        if (gold > int.MaxValue || gold < 0) // Check gold overflow
        {
            _player.SendDefinedText(DefineText.TID_GAME_TOOMANYMONEY_USE_PERIN);
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
        using UpdateParamPointSnapshot goldUpdateSnapshot = new(_player, DefineAttributes.DST_GOLD, Amount);

        _player.Send(goldUpdateSnapshot);
    }
}