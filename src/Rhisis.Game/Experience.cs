using Rhisis.Core.Configuration;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using System;
using System.Collections.Generic;

namespace Rhisis.Game;

public class Experience
{
    private static readonly Dictionary<DefineJob.JobType, int> _experienceLevelLimits = new()
    {
        { DefineJob.JobType.JTYPE_BASE, (int)DefineJob.JobMax.MAX_JOB_LEVEL },
        { DefineJob.JobType.JTYPE_EXPERT, (int)DefineJob.JobMax.MAX_JOB_LEVEL + (int)DefineJob.JobMax.MAX_EXP_LEVEL },
        { DefineJob.JobType.JTYPE_PRO, (int)DefineJob.JobMax.MAX_LEVEL },
        { DefineJob.JobType.JTYPE_MASTER, (int)DefineJob.JobMax.MAX_LEVEL },
        { DefineJob.JobType.JTYPE_HERO, (int)DefineJob.JobMax.MAX_LEGEND_LEVEL }
    };

    private readonly Player _player;

    public long Amount { get; private set; }

    public Experience(Player player)
    {
        _player = player;
    }

    public void Initialize(long initialExperience) => Amount = initialExperience;

    public bool Increase(long amount)
    {
        if (_player.Mode.HasFlag(ModeType.MODE_EXPUP_STOP))
        {
            return false;
        }

        // TODO: experience to party
        long experience = CalculateExtraExperience(amount);
        bool hasLevelUp = GiveExperienceToPlayer(experience);

        if (hasLevelUp)
        {
            _player.Health.RegenerateAll();
            SendLevelUpPackets();
        }

        SendExperiencePacket(hasLevelUp);
        // TODO: send packet to friends, messenger, guild, couple, party, etc...

        return true;
    }

    public bool Decrease(long amount)
    {
        if (_player.Mode.HasFlag(ModeType.MODE_EXPUP_STOP))
        {
            return false;
        }

        throw new NotImplementedException();
    }

    public void ApplyDeathPenality(bool sendToPlayer = true)
    {
        if (GameOptions.Current.DeathPenalityEnabled)
        {
            decimal expLossPercent = GameResources.Current.Penalities.GetDecExpPenality(_player.Level);

            if (expLossPercent <= 0)
            {
                return;
            }

            Amount -= Amount * (long)(expLossPercent / 100m);
            _player.DeathLevel = _player.Level;

            if (Amount < 0)
            {
                if (GameResources.Current.Penalities.GetLevelDownPenality(_player.Level))
                {
                    CharacterExpTableProperties previousLevelExp = GameResources.Current.ExperienceTable.GetCharacterExp(_player.Level - 1);

                    _player.Level--;
                    Amount = previousLevelExp.Exp + Amount;
                }
                else
                {
                    Amount = 0;
                }
            }

            if (sendToPlayer)
            {
                SendExperiencePacket(false);
            }
        }
    }

    public void Reset()
    {
        Amount = 0;
    }

    /// <summary>
    /// Give experience to a player and returns a boolean value that indicates if the player has level up.
    /// </summary>
    /// <param name="player">Player entity.</param>
    /// <param name="experience">Experience to give.</param>
    /// <returns>True if the player has level up; false otherwise.</returns>
    private bool GiveExperienceToPlayer(long experience)
    {
        if (PlayerHasReachedMaxLevel())
        {
            Amount = 0;

            return false;
        }

        var nextLevel = _player.Level + 1;
        CharacterExpTableProperties nextLevelExpTable = GameResources.Current.ExperienceTable.GetCharacterExp(nextLevel);
        Amount += experience;

        if (Amount >= nextLevelExpTable.Exp)
        {
            var remainingExp = Amount - nextLevelExpTable.Exp;

            ProcessLevelUp((ushort)nextLevelExpTable.Gp);

            if (remainingExp > 0)
            {
                // Calling the method recursively to give XP to to the player after he levels up
                // if there is some remaining exp to give.
                GiveExperienceToPlayer(remainingExp);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Calculates extra experience with scrolls, events, skill bonus, etc...
    /// </summary>
    /// <param name="player">Player entity.</param>
    /// <param name="experience">Current experience.</param>
    /// <returns>Base experience with extra experience.</returns>
    private long CalculateExtraExperience(long baseExperience)
    {
        // TODO: add exp scrolls logic here

        return baseExperience;
    }

    /// <summary>
    /// Checks if the player has reached the maximum level for its job.
    /// </summary>
    /// <param name="player">Current player.</param>
    /// <returns>True if the player has reached the maximum level for its job; false otherwise.</returns>
    private bool PlayerHasReachedMaxLevel()
    {
        if (!_experienceLevelLimits.TryGetValue(_player.Job.Type, out int limitLevel))
        {
            return false;
        }

        return _player.Level >= limitLevel;
    }

    /// <summary>
    /// Process the level up logic and reward attribution.
    /// </summary>
    /// <param name="player">Player entity.</param>
    /// <param name="statPoints">Statistics points.</param>
    private void ProcessLevelUp(ushort statPoints)
    {
        _player.Level += 1;

        if (_player.Level != _player.DeathLevel)
        {
            _player.Skills.AddSkillPointsForLevelUp(_player.Level, sendToPlayer: false);
            _player.AvailablePoints += statPoints;
        }

        Amount = 0;
    }

    private void SendExperiencePacket(bool sendLearningPoints)
    {
        using SetExperienceSnapshot playerSnapshots = new(_player);

        if (sendLearningPoints)
        {
            playerSnapshots.Merge(new SetGrowthLearningPointSnapshot(_player));
        }

        _player.Send(playerSnapshots);
    }

    private void SendLevelUpPackets()
    {
        using SetLevelSnapshot levelSnapshot = new(_player, _player.Level);

        _player.SendToVisible(levelSnapshot);
    }
}
