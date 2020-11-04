using Rhisis.Game.Abstractions.Entities;
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
        /// <param name="objectMessageType">Object message type.</param>
        /// <param name="sendHitPoints">Boolean value that indiciates if the system should send the current hit points.</param>
        void Die(IMover killer, ObjectMessageType objectMessageType = ObjectMessageType.OBJMSG_ATK1, bool sendHitPoints = false);

        /// <summary>
        /// Inflict a given amount of damages to the current mover.
        /// </summary>
        /// <param name="attacker">Mover that attacked the current mover.</param>
        /// <param name="damages">Amount of damages to inflict.</param>
        /// <param name="attackFlags">Attack flags.</param>
        /// <param name="objectMessageType">Attack message type.</param>
        void SufferDamages(IMover attacker, int damages, AttackFlags attackFlags = AttackFlags.AF_GENERIC, ObjectMessageType objectMessageType = ObjectMessageType.OBJMSG_ATK1);

        /// <summary>
        /// Regenerates a small amount of the current mover health.
        /// </summary>
        void IdleHeal();
    }
}
