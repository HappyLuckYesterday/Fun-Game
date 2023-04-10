using System.Collections.Generic;

namespace Rhisis.Game.Common.Resources;

public class DeathPenalityProperties
{
    /// <summary>
    /// Gets the revival penality data.
    /// </summary>
    public IEnumerable<PenalityProperties> RevivalPenality { get; set; }

    /// <summary>
    /// Gets or sets the Dec experience penality data.
    /// </summary>
    public IEnumerable<PenalityProperties> DecExpPenality { get; set; }

    /// <summary>
    /// Gets the level down penality data.
    /// </summary>
    public IEnumerable<PenalityProperties> LevelDownPenality { get; set; }
}
