using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public class AttributeSystem : GameFeature, IAttributeSystem
    {
        public void ResetAttribute(IMover entity, DefineAttributes attribute, int value, bool sendToEntity = true)
        {
            throw new System.NotImplementedException();
        }

        public void SetAttribute(IMover entity, DefineAttributes attribute, int value, bool sendToEntity = true)
        {
            throw new System.NotImplementedException();
        }
    }
}
