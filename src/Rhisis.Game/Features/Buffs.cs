﻿using Microsoft.EntityFrameworkCore.Internal;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Sylver.Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Features
{
    public class Buffs : GameFeature, IBuffs
    {
        private readonly IList<IBuff> _buffs;

        public IMover Owner { get; }

        public Buffs(IMover owner)
        {
            _buffs = new List<IBuff>();
            Owner = owner;
        }

        public bool Add(IBuff buff)
        {
            if (buff.HasExpired)
            {
                return false;
            }

            if (Contains(buff) && buff is IBuffSkill buffSkill)
            {
                IBuffSkill existingBuff = _buffs.OfType<IBuffSkill>().FirstOrDefault(x => x.Id == buffSkill.Id);

                if (existingBuff is null)
                {
                    return false;
                }

                if (existingBuff.SkillLevel == buffSkill.SkillLevel)
                {
                    existingBuff.RemainingTime = buffSkill.RemainingTime;
                    return true;
                }
                else if (existingBuff.SkillLevel > buffSkill.SkillLevel)
                {
                    return true;
                }

                Remove(existingBuff);
            }

            _buffs.Add(buff);
            

            return true;
        }

        public bool Remove(IBuff buff)
        {
            if (_buffs.Remove(buff))
            {
                foreach (KeyValuePair<DefineAttributes, int> attribute in buff.Attributes)
                {
                    Owner.Attributes.Decrease(attribute.Key, attribute.Value);
                }

                return true;
            }

            return false;
        }

        public void RemoveAll()
        {
            IEnumerable<IBuff> buffs = _buffs.ToList();

            foreach (IBuff buff in buffs)
            {
                Remove(buff);
            }
        }

        public bool Contains(IBuff buff)
        {
            if (buff is null)
            {
                return false;
            }

            return _buffs.Any(x => x.Id == buff.Id);
        }

        public void Update()
        {
            if (_buffs.Any())
            {
                IEnumerable<IBuff> buffs = _buffs.ToList();

                foreach (IBuff buff in buffs)
                {
                    buff.DecreaseTime();

                    if (buff.HasExpired)
                    {
                        // TODO: do something when a buff has expired
                    }
                }

                IEnumerable<IBuff> expiredBuffs = buffs.Where(x => x.HasExpired);

                foreach (IBuff buff in expiredBuffs)
                {
                    Remove(buff);
                }
            }
        }

        public void Serialize(INetPacketStream packet)
        {
            IEnumerable<IBuff> activeBuffs = _buffs.Where(x => !x.HasExpired);

            packet.WriteInt32(activeBuffs.Count());

            foreach (IBuff buff in activeBuffs)
            {
                buff.Serialize(packet);
            }
        }

        public IEnumerator<IBuff> GetEnumerator() => _buffs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _buffs.GetEnumerator();
    }
}
