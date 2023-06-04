using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rhisis.Game;

public class Buff
{
    /// <summary>
    /// Gets the buff unique id.
    /// </summary>
    public uint Id { get; } = FFRandom.GenerateUniqueId();

    /// <summary>
    /// Gets the buff type.
    /// </summary>
    public virtual BuffType Type { get; }

    /// <summary>
    /// Gets the buff owner.
    /// </summary>
    public Mover Owner { get; }

    /// <summary>
    /// Gets the buff remaining time before expiration.
    /// </summary>
    public int RemainingTime { get; set; }

    /// <summary>
    /// Gets a boolean value that indicates if the buff has expired.
    /// </summary>
    public bool HasExpired => RemainingTime <= 0;

    /// <summary>
    /// Gets the buff attributes.
    /// </summary>
    public IReadOnlyDictionary<DefineAttributes, int> Attributes { get; }

    public Buff(Mover owner, IDictionary<DefineAttributes, int> attributes)
    {
        Owner = owner;
        Attributes = new Dictionary<DefineAttributes, int>(attributes);
    }

    /// <summary>
    /// Decreases the buff remaining time.
    /// </summary>
    /// <param name="time">Time in seconds.</param>
    public void DecreaseTime(int time = 1)
    {
        RemainingTime -= time * 1000;
    }

    public bool Equals([AllowNull] Buff other) => Id == other?.Id;

    public virtual void Serialize(FFPacket packet)
    {
        // Nothing to do.
    }
}
