using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Attributes
{
    public interface IAttributeSystem
    {
        void SetAttribute(ILivingEntity entity, DefineAttributes attribute, int value, bool sendToEntity = true);

        void ResetAttribute(ILivingEntity entity, DefineAttributes attribute, int value, bool sendToEntity = true);
    }
}
