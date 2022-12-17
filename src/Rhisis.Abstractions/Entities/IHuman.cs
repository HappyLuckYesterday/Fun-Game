using Rhisis.Abstractions.Features;

namespace Rhisis.Abstractions.Entities;

/// <summary>
/// Describes the components of a human entity.
/// </summary>
public interface IHuman : IMover
{
    /// <summary>
    /// Gets the human visual appearance.
    /// </summary>
    IHumanVisualAppearance Appearence { get; }
}
