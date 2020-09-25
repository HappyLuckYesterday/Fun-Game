using Microsoft.Extensions.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Components
{
    public class AttributesComponent : IAttributes
    {
        private readonly IMover _mover;
        private readonly IDictionary<DefineAttributes, int> _attributes;
        private readonly Lazy<IAttributeSystem> _attributeSystem;

        public AttributesComponent(IMover mover)
        {
            _mover = mover;
            _attributes = new Dictionary<DefineAttributes, int>();
            _attributeSystem = new Lazy<IAttributeSystem>(() => mover.Systems.GetService<IAttributeSystem>());
        }

        public int Get(DefineAttributes attribute, int defaultValue = 0)
        {
            throw new System.NotImplementedException();
        }

        public void Set(DefineAttributes attribute, int value)
        {
            throw new System.NotImplementedException();
        }

        public void IncreaseAttribute(DefineAttributes attribute, int value)
        {
            throw new System.NotImplementedException();
        }

        public void DecreaseAttribute(DefineAttributes attribute, int value)
        {
            throw new System.NotImplementedException();
        }
    }

    public class HealthComponent : IHealth
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
