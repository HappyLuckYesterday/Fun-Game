using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using System.Collections.Generic;

namespace Rhisis.Game;

public sealed class Attributes
{
    private readonly Mover _mover;
    private readonly Dictionary<DefineAttributes, int> _attributes = new();

    public Attributes(Mover mover)
    {
        _mover = mover;
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
            case DefineAttributes.DST_RESIST_ALL:
                Increase(DefineAttributes.DST_RESIST_FIRE, value, sendToEntity);
                Increase(DefineAttributes.DST_RESIST_ELECTRICITY, value, sendToEntity);
                Increase(DefineAttributes.DST_RESIST_WATER, value, sendToEntity);
                Increase(DefineAttributes.DST_RESIST_WIND, value, sendToEntity);
                Increase(DefineAttributes.DST_RESIST_EARTH, value, sendToEntity);
                return;
            case DefineAttributes.DST_STAT_ALLUP:
                Increase(DefineAttributes.DST_STR, value, sendToEntity);
                Increase(DefineAttributes.DST_STA, value, sendToEntity);
                Increase(DefineAttributes.DST_DEX, value, sendToEntity);
                Increase(DefineAttributes.DST_INT, value, sendToEntity);
                return;
        }

        if (value != 0)
        {
            switch (attribute)
            {
                case DefineAttributes.DST_CHRSTATE:
                case DefineAttributes.DST_IMMUNITY:
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
            case DefineAttributes.DST_RESIST_ALL:
                Decrease(DefineAttributes.DST_RESIST_FIRE, value, sendToEntity);
                Decrease(DefineAttributes.DST_RESIST_ELECTRICITY, value, sendToEntity);
                Decrease(DefineAttributes.DST_RESIST_WATER, value, sendToEntity);
                Decrease(DefineAttributes.DST_RESIST_WIND, value, sendToEntity);
                Decrease(DefineAttributes.DST_RESIST_EARTH, value, sendToEntity);
                return;
            case DefineAttributes.DST_STAT_ALLUP:
                Decrease(DefineAttributes.DST_STR, value, sendToEntity);
                Decrease(DefineAttributes.DST_STA, value, sendToEntity);
                Decrease(DefineAttributes.DST_DEX, value, sendToEntity);
                Decrease(DefineAttributes.DST_INT, value, sendToEntity);
                return;
        }

        if (value != 0)
        {
            if (attribute == DefineAttributes.DST_CHRSTATE)
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
        if (_mover is Player player)
        {
            using UpdateDestParamSnapshot snapshot = new(player, attribute, value);

            player.Send(snapshot);
        }
    }
}