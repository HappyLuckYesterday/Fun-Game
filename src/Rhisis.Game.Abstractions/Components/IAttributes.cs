using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Components
{
    public interface IAttributes
    {
        int Get(DefineAttributes attribute, int defaultValue = 0);

        void Set(DefineAttributes attribute, int value);

        void IncreaseAttribute(DefineAttributes attribute, int value);

        void DecreaseAttribute(DefineAttributes attribute, int value);
    }
}
