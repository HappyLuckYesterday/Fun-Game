using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;
using System.Collections.Generic;

namespace Rhisis.Game.Features
{
    public class Attributes : GameFeature, IAttributes
    {
        private readonly IMover _mover;
        private readonly Dictionary<DefineAttributes, int> _attributes;

        public Attributes(IMover mover)
        {
            _mover = mover;
            _attributes = new Dictionary<DefineAttributes, int>();
        }

        public int Get(DefineAttributes attribute, int defaultValue = 0)
        {
            return _attributes.TryGetValue(attribute, out int value) ? value : defaultValue;
        }

        public void Set(DefineAttributes attribute, int value, bool sendToEntity = true)
        {
            if (_attributes.ContainsKey(attribute))
            {
                _attributes[attribute] = value;
            }
            else
            {
                _attributes.Add(attribute, value);
            }
        }

        public void Increase(DefineAttributes attribute, int value, bool sendToEntity = true)
        {
            switch (attribute)
            {
                case DefineAttributes.RESIST_ALL:
                    Increase(DefineAttributes.RESIST_FIRE, value, sendToEntity);
                    Increase(DefineAttributes.RESIST_ELECTRICITY, value, sendToEntity);
                    Increase(DefineAttributes.RESIST_WATER, value, sendToEntity);
                    Increase(DefineAttributes.RESIST_WIND, value, sendToEntity);
                    Increase(DefineAttributes.RESIST_EARTH, value, sendToEntity);
                    return;
                case DefineAttributes.STAT_ALLUP:
                    Increase(DefineAttributes.STR, value, sendToEntity);
                    Increase(DefineAttributes.STA, value, sendToEntity);
                    Increase(DefineAttributes.DEX, value, sendToEntity);
                    Increase(DefineAttributes.INT, value, sendToEntity);
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
                            Set(attribute, Get(attribute) | value);
                        }
                        break;
                    default:
                        Set(attribute, Get(attribute) + value);
                        break;
                }

                if (sendToEntity)
                {
                    SendAttributeValue(attribute, Get(attribute));
                }
            }
        }

        public void Decrease(DefineAttributes attribute, int value, bool sendToEntity = true)
        {
            switch (attribute)
            {
                case DefineAttributes.RESIST_ALL:
                    Decrease(DefineAttributes.RESIST_FIRE, value, sendToEntity);
                    Decrease(DefineAttributes.RESIST_ELECTRICITY, value, sendToEntity);
                    Decrease(DefineAttributes.RESIST_WATER, value, sendToEntity);
                    Decrease(DefineAttributes.RESIST_WIND, value, sendToEntity);
                    Decrease(DefineAttributes.RESIST_EARTH, value, sendToEntity);
                    return;
                case DefineAttributes.STAT_ALLUP:
                    Decrease(DefineAttributes.STR, value, sendToEntity);
                    Decrease(DefineAttributes.STA, value, sendToEntity);
                    Decrease(DefineAttributes.DEX, value, sendToEntity);
                    Decrease(DefineAttributes.INT, value, sendToEntity);
                    return;
            }

            if (value != 0)
            {
                if (attribute == DefineAttributes.CHRSTATE)
                {
                    Set(attribute, Get(attribute) & ~value);
                    //_attributes[attribute] &= ~value;
                }
                else
                {
                    Set(attribute, Get(attribute) - value);
                }
            }

            if (sendToEntity)
            {
                SendAttributeValue(attribute, Get(attribute));
            }
        }

        private void SendAttributeValue(DefineAttributes attribute, int value)
        {
            if (_mover is IPlayer player)
            {
                using var updateAttributeSnapshot = new UpdateDestParamSnapshot(player, attribute, value);

                player.Connection.Send(updateAttributeSnapshot);
            }
        }
    }
}
