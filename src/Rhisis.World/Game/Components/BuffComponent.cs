using Rhisis.World.Game.Structures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class BuffComponent : IEnumerable<Buff>
    {
        private readonly List<Buff> _buffs;

        public BuffComponent()
        {
            _buffs = new List<Buff>();
        }

        public void Add(Buff buff)
        {
            if (buff is BuffSkill buffSkill)
            {
                BuffSkill existingBuff = _buffs.OfType<BuffSkill>().FirstOrDefault(x => x.SkillId == buffSkill.SkillId);

                if (existingBuff != null && existingBuff.SkillLevel < buffSkill.SkillLevel)
                {
                    Remove(existingBuff);
                    _buffs.Add(buff);
                }
                else if (existingBuff == null)
                {
                    _buffs.Add(buff);
                }
            }
        }

        public void Remove(Buff buff)
        {
            _buffs.Remove(buff);
        }

        public IEnumerator<Buff> GetEnumerator() => _buffs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _buffs.GetEnumerator();
    }
}
