using System.Diagnostics;

namespace Rhisis.Game;

[DebuggerDisplay("{Value}")]
public sealed class ItemSerialNumber
{
    public int Value { get; }
}
