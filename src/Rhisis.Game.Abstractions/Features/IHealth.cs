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
    }
}
