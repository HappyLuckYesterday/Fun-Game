using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IAttributeSystem
    {
        void SetAttribute(IMover entity, DefineAttributes attribute, int value, bool sendToEntity = true);

        void ResetAttribute(IMover entity, DefineAttributes attribute, int value, bool sendToEntity = true);
    }
}
