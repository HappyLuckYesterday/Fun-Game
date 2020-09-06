using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Components
{
    public class HealthComponent
    {
        private readonly IMover _mover;

        /// <summary>
        /// Gets a boolean value that indicates if the current entity is dead.
        /// </summary>
        public bool IsDead => Hp < 0;

        /// <summary>
        /// Gets or sets the Hit points.
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        /// Gets or sets the Mana points.
        /// </summary>
        public int Mp { get; set; }

        /// <summary>
        /// Gets or sets the Fatigue points.
        /// </summary>
        public int Fp { get; set; }

        /// <summary>
        /// Creates a new <see cref="HealthComponent"/> instance.
        /// </summary>
        /// <param name="player">Current player.</param>
        public HealthComponent(IMover mover)
        {
            _mover = mover;
        }
    }
}
