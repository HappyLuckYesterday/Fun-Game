using Rhisis.Game.Common;
using Rhisis.Game.Resources.Properties;
using System;

namespace Rhisis.Game.Entities;

public class Mover : WorldObject
{
    public override WorldObjectType Type => WorldObjectType.Mover;

    /// <summary>
    /// Gets or sets the mover speed factor.
    /// </summary>
    public float SpeedFactor { get; set; } = 1;

    /// <summary>
    /// Gets or sets the mover destination position.
    /// </summary>
    public Vector3 DestinationPosition { get; set; }

    /// <summary>
    /// Gets the mover properties.
    /// </summary>
    public MoverProperties Properties { get; }

    /// <summary>
    /// Gets or sets the mover level.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Gets a boolean value that indicates if the mover is dead.
    /// </summary>
    public bool IsDead => Health.Hp <= 0;

    /// <summary>
    /// Gets the mover health.
    /// </summary>
    public Health Health { get; init; }

    /// <summary>
    /// Gets the attributes.
    /// </summary>
    public Attributes Attributes { get; }

    /// <summary>
    /// Gets the player's statistics.
    /// </summary>
    public Statistics Statistics { get; init; }

    protected Mover(MoverProperties properties)
    {
        Properties = properties ?? throw new ArgumentNullException(nameof(properties), "Cannot create a mover with no properties.");
        Health = new Health(this);
        Attributes = new Attributes(this);
        Statistics = new Statistics(this);
    }
}
