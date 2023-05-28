using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game;

public sealed class Statistics
{
    private readonly Mover _mover;

    /// <summary>
    /// Gets or sets the original strength points.
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// Gets or sets the original stamina points.
    /// </summary>
    public int Stamina { get; set; }

    /// <summary>
    /// Gets or sets the original dexterity points.
    /// </summary>
    public int Dexterity { get; set; }

    /// <summary>
    /// Gets or sets the orginal intelligence points.
    /// </summary>
    public int Intelligence { get; set; }

    public Statistics(Mover owner)
    {
        _mover = owner ?? throw new ArgumentNullException(nameof(owner), "Cannot assign statistics an unknown mover instance.");
        Strength = owner.Properties.Strength;
        Stamina = owner.Properties.Stamina;
        Dexterity = owner.Properties.Dexterity;
        Intelligence = owner.Properties.Intelligence;
    }
}
