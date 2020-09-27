using Microsoft.Extensions.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Systems;
using System;

namespace Rhisis.Game.Abstractions.Components
{
    public class Health : IHealth
    {
        private readonly IMover _mover;
        private readonly Lazy<IHealthFormulas> _healthFormulas;

        public bool IsDead => Hp < 0;

        public int Hp { get; set; }

        public int Mp { get; set; }

        public int Fp { get; set; }

        public int MaxHp => _healthFormulas.Value.GetMaxHp(_mover);

        public int MaxMp => _healthFormulas.Value.GetMaxMp(_mover);

        public int MaxFp => _healthFormulas.Value.GetMaxFp(_mover);

        /// <summary>
        /// Creates a new <see cref="Health"/> instance.
        /// </summary>
        /// <param name="player">Current player.</param>
        public Health(IMover mover)
        {
            _mover = mover;
            _healthFormulas = new Lazy<IHealthFormulas>(() => mover.Systems.GetService<IHealthFormulas>());
        }
    }
}
