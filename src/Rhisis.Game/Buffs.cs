using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public sealed class Buffs : IEnumerable<Buff>, IPacketSerializer
{
    private readonly List<Buff> _buffs = new();

    /// <summary>
    /// Gets the buffs owner.
    /// </summary>
    public Mover Owner { get; }
 
    /// <summary>
    /// Creates a new <see cref="Buffs"/> instance.
    /// </summary>
    /// <param name="owner"></param>
    public Buffs(Mover owner)
    {
        Owner = owner;
    }

    /// <summary>
    /// Adds a buff to the owner's buff list.
    /// </summary>
    /// <param name="buff">Buff to add.</param>
    /// <returns>Buff result.</returns>
    public BuffResultType Add(Buff buff)
    {
        if (buff.HasExpired)
        {
            return BuffResultType.None;
        }

        if (Contains(buff) && buff is BuffSkill buffSkill)
        {
            BuffSkill existingBuff = _buffs.OfType<BuffSkill>().FirstOrDefault(x => x.SkillId == buffSkill.SkillId);

            if (existingBuff is null)
            {
                return BuffResultType.None;
            }

            if (existingBuff.SkillLevel == buffSkill.SkillLevel)
            {
                existingBuff.RemainingTime = buffSkill.RemainingTime;
                return BuffResultType.Updated;
            }
            else if (existingBuff.SkillLevel > buffSkill.SkillLevel)
            {
                return BuffResultType.None;
            }

            Remove(existingBuff);
        }

        _buffs.Add(buff);

        foreach (KeyValuePair<DefineAttributes, int> attribute in buff.Attributes)
        {
            Owner.Attributes.Increase(attribute.Key, attribute.Value);
        }

        return BuffResultType.Added;
    }

    /// <summary>
    /// Removes the given buff from the owner's buff list.
    /// </summary>
    /// <param name="buff">Buff to remove.</param>
    /// <returns>True if the buff has been removed; false otherwise.</returns>
    public bool Remove(Buff buff)
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

    /// <summary>
    /// Remove all buffs.
    /// </summary>
    public void RemoveAll()
    {
        IEnumerable<Buff> buffs = _buffs.ToList();

        foreach (Buff buff in buffs)
        {
            Remove(buff);
        }
    }

    /// <summary>
    /// Check if the given buff is present in the owner's buff list.
    /// </summary>
    /// <param name="buff">Buff.</param>
    /// <returns>True if the buff exists; false otherwise.</returns>
    public bool Contains(Buff buff)
    {
        if (buff is null)
        {
            return false;
        }

        if (buff is BuffSkill buffSkill)
        {
            return _buffs.OfType<BuffSkill>().Any(x => x.SkillId == buffSkill.SkillId);
        }

        return _buffs.Any(x => x.Id == buff.Id);
    }

    /// <summary>
    /// Updates the buff list.
    /// </summary>
    public void Update()
    {
        if (_buffs.Any())
        {
            IEnumerable<Buff> buffs = _buffs.ToList();

            foreach (Buff buff in buffs)
            {
                buff.DecreaseTime();

                if (buff.HasExpired)
                {
                    Remove(buff);
                }
            }

            IEnumerable<Buff> expiredBuffs = buffs.Where(x => x.HasExpired);

            foreach (Buff buff in expiredBuffs)
            {
                Remove(buff);
            }
        }
    }

    /// <summary>
    /// Serialies the buff list.
    /// </summary>
    /// <param name="packet"></param>
    public void Serialize(FFPacket packet)
    {
        IEnumerable<Buff> activeBuffs = _buffs.Where(x => !x.HasExpired);

        packet.WriteInt32(activeBuffs.Count());

        foreach (Buff buff in activeBuffs)
        {
            buff.Serialize(packet);
        }
    }

    public IEnumerator<Buff> GetEnumerator() => _buffs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _buffs.GetEnumerator();
}
