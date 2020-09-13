using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Components
{
    public interface IAttributes
    {
        int Get(DefineAttributes attribute);

        void Set(DefineAttributes attribute, int value);

        void IncreaseAttribute(DefineAttributes attribute, int value);

        void DecreaseAttribute(DefineAttributes attribute, int value);
    }
}
