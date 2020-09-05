using Rhisis.Game.Abstractions.Components;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IHuman : IMover
    {
        VisualAppearenceComponent Appearence { get; }
    }
}
