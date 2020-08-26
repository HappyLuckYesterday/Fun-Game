using Rhisis.Game.Components;

namespace Rhisis.Game.Entities
{
    public interface IHuman : IMover
    {
        VisualAppearenceComponent Appearence { get; }
    }
}
