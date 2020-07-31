using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Health;

namespace Rhisis.World.Systems.Attributes
{
    [Injectable]
    public class AttributeSystem : IAttributeSystem
    {
        private readonly IHealthSystem _healthSystem;
        private readonly IMoverPacketFactory _moverPacketFactory;

        public AttributeSystem(IHealthSystem healthSystem, IMoverPacketFactory moverPacketFactory)
        {
            _healthSystem = healthSystem;
            _moverPacketFactory = moverPacketFactory;
        }

        public void SetAttribute(ILivingEntity entity, DefineAttributes attribute, int value, bool sendToEntity = true)
        {
            switch (attribute)
            {
                case DefineAttributes.HP:
                case DefineAttributes.MP:
                case DefineAttributes.FP:
                    _healthSystem.IncreasePoints(entity, attribute, value);
                    break;
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

        public void ResetAttribute(ILivingEntity entity, DefineAttributes attribute, int value, bool sendToEntity = true)
        {
            switch (attribute)
            {
                case DefineAttributes.RESIST_ALL:
                    ResetAttribute(entity, DefineAttributes.RESIST_FIRE, value, sendToEntity);
                    ResetAttribute(entity, DefineAttributes.RESIST_ELECTRICITY, value, sendToEntity);
                    ResetAttribute(entity, DefineAttributes.RESIST_WATER, value, sendToEntity);
                    ResetAttribute(entity, DefineAttributes.RESIST_WIND, value, sendToEntity);
                    ResetAttribute(entity, DefineAttributes.RESIST_EARTH, value, sendToEntity);
                    return;
                case DefineAttributes.STAT_ALLUP:
                    ResetAttribute(entity, DefineAttributes.STR, value, sendToEntity);
                    ResetAttribute(entity, DefineAttributes.STA, value, sendToEntity);
                    ResetAttribute(entity, DefineAttributes.DEX, value, sendToEntity);
                    ResetAttribute(entity, DefineAttributes.INT, value, sendToEntity);
                    return;
            }

            if (value != 0)
            {
                if (attribute == DefineAttributes.CHRSTATE)
                {
                    entity.Attributes[attribute] &= ~value;
                }
                else
                {
                    entity.Attributes[attribute] -= value;
                }
            }

            if (sendToEntity)
            {
                _moverPacketFactory.SendResetAttribute(entity, attribute, value);
            }
        }
    }
}
