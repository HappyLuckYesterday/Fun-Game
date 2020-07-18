using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Attributes
{
    [Injectable]
    public class AttributeSystem : IAttributeSystem
    {
        private readonly IMoverPacketFactory _moverPacketFactory;

        public AttributeSystem(IMoverPacketFactory moverPacketFactory)
        {
            _moverPacketFactory = moverPacketFactory;
        }

        public void SetAttribute(ILivingEntity entity, DefineAttributes attribute, int value, bool sendToEntity = true)
        {
            switch (attribute)
            {
                case DefineAttributes.RESIST_ALL:
                    SetAttribute(entity, DefineAttributes.RESIST_FIRE, value, sendToEntity);
                    SetAttribute(entity, DefineAttributes.RESIST_ELECTRICITY, value, sendToEntity);
                    SetAttribute(entity, DefineAttributes.RESIST_WATER, value, sendToEntity);
                    SetAttribute(entity, DefineAttributes.RESIST_WIND, value, sendToEntity);
                    SetAttribute(entity, DefineAttributes.RESIST_EARTH, value, sendToEntity);
                    return;
                case DefineAttributes.STAT_ALLUP:
                    SetAttribute(entity, DefineAttributes.STR, value, sendToEntity);
                    SetAttribute(entity, DefineAttributes.STA, value, sendToEntity);
                    SetAttribute(entity, DefineAttributes.DEX, value, sendToEntity);
                    SetAttribute(entity, DefineAttributes.INT, value, sendToEntity);
                    return;
            }

            // TODO: set attribute
            if (value != 0)
            {
                switch (attribute)
                {
                    case DefineAttributes.CHRSTATE:
                    case DefineAttributes.IMMUNITY:
                        if (value != -1)
                        {
                            entity.Attributes[attribute] |= value;
                        }
                        break;
                    default:
                        entity.Attributes[attribute] += value;
                        break;
                }

                if (sendToEntity)
                {
                    _moverPacketFactory.SendUpdateAttributes(entity, attribute, entity.Attributes[attribute]);
                }
            }
        }
    }
}
