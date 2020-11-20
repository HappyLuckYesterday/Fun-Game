using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Features
{
    public interface IHealth
    {
        /// <summary>
        /// Gets a boolean value that indicates if the current entity is dead.
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// Gets or sets the Hit points.
        /// </summary>
        int Hp { get; set; }

        /// <summary>
        /// Gets or sets the Mana points.
        /// </summary>
        int Mp { get; set; }

        /// <summary>
        /// Gets or sets the Fatigue points.
        /// </summary>
        int Fp { get; set; }

        /// <summary>
        /// Gets the maximum hit points.
        /// </summary>
        int MaxHp { get; }

        /// <summary>
        /// Gets the maximum Mana poitns.
        /// </summary>
        int MaxMp { get; }

        /// <summary>
        /// Gets the maxium Fatigue points.
        /// </summary>
        int MaxFp { get; }

        /// <summary>
        /// Regenerates all points.
        /// </summary>
        void RegenerateAll();

        /// <summary>
        /// Kills the current mover and makes it die.
        /// </summary>
        /// <param name="killer">Mover that killed the current mover.</param>
        /// <param name="attackType">The type of attack that caused the death.</param>
        /// <param name="sendHitPoints">Boolean value that indiciates if the system should send the current hit points.</param>
        void Die(IMover killer, AttackType attackType, bool sendHitPoints = false);

        /// <summary>
        /// Inflict a given amount of damages to the current mover.
        /// </summary>
        /// <param name="attacker">Mover that attacked the current mover.</param>
        /// <param name="damages">Amount of damages to inflict.</param>
        /// <param name="attackType">Attack type that causes the damages</param>
        /// <param name="attackFlags">Attack flags.</param>
        void SufferDamages(IMover attacker, int damages, AttackType attackType, AttackFlags attackFlags = AttackFlags.AF_GENERIC);

        /// <summary>
        /// Regenerates a small amount of the current mover health.
        /// </summary>
        void IdleHeal();

        /// <summary>
        /// Applies revival health penality to the player.
        /// </summary>
        /// <param name="send">Sends the update to the visible movers of the current mover.</param>
        void ApplyDeathRecovery(bool send = true);

        /// <summary>
        /// Gets the current health points based on the given attribute.
        /// </summary>
        /// <param name="attribute">Health attribute.</param>
        /// <returns></returns>
        int GetCurrent(DefineAttributes attribute);

        /// <summary>
        /// Sets the current health points based on the given attribute.
        /// </summary>
        /// <param name="attribute">Health attribute.</param>
        /// <param name="value">Health points value.</param>
        /// <param name="send">Boolean value that indicates if the changes should be sent.</param>
        void SetCurrent(DefineAttributes attribute, int value, bool send = true);

        /// <summary>
        /// Gets the maximum health points based on the given attribute.
        /// </summary>
        /// <param name="attribute">Health attribute.</param>
        /// <returns></returns>
        int GetMaximum(DefineAttributes attribute);
    }
}
