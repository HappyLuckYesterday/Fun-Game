using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Features.AttackArbiters.Reducers
{
    public class MeleeAttackReducer : AttackReducerBase
    {
        public MeleeAttackReducer(IMover attacker, IMover defender) 
            : base(attacker, defender)
        {
        }

        public override AttackResult ReduceDamages(AttackResult attackResult)
        {
            int damages = attackResult.Damages;
            AttackFlags flags = attackResult.Flags;

            if (flags.HasFlag(AttackFlags.AF_MAGICSKILL))
            {
                damages -= Defender.Attributes.Get(DefineAttributes.RESIST_MAGIC);
            }

            var defense = 0;
            bool isGenericAttack = flags.HasFlag(AttackFlags.AF_GENERIC) || (Attacker is IPlayer && Defender is IPlayer);

            int level = Defender.Level;

            if (isGenericAttack)
            {
                float defenseFactor = Defender is IPlayer player ? player.Job.DefenseFactor : 1f;
                int armorDefense = Defender.Defense.GetDefense();
                int stamina = Defender.Statistics.Stamina + Defender.Attributes.Get(DefineAttributes.STA);

                defense = (int)((level * 2 + stamina / 2) / 2.8f - 4 + (stamina - 14) * defenseFactor);
                defense += armorDefense / 4;
                defense += Defender.Attributes.Get(DefineAttributes.ADJDEF);

                defense = Math.Max(defense, 0);
            }
            else if (Defender is IMonster monster)
            {
                int monsterDefenseArmor = flags.HasFlag(AttackFlags.AF_MAGIC) ? monster.Data.MagicResitance : monster.Data.NaturalArmor;

                defense = (int)(monsterDefenseArmor / 7f + 1);
            }
            else if (Defender is IPlayer)
            {
                int stamina = Defender.Statistics.Stamina + Defender.Attributes.Get(DefineAttributes.STA);
                int dexterity = Defender.Statistics.Dexterity + Defender.Attributes.Get(DefineAttributes.DEX);

                if (Attacker is IPlayer)
                {
                    if (flags.HasFlag(AttackFlags.AF_MAGIC))
                    {
                        defense = (int)(Defender.Attributes.Get(DefineAttributes.INT) * 9.04f + level * 35.98f);
                    }
                    else
                    {
                        int armorDefense = Defender.Defense.GetDefense() + Defender.Attributes.Get(DefineAttributes.ADJDEF);

                        defense = (int)((armorDefense * 2.3f) + ((level + (stamina / 2) + dexterity) / 2.8f) - 4 + level * 2);
                    }
                }
                else
                {
                    int armorDefense = Defender.Defense.GetDefense() / 4 + Defender.Attributes.Get(DefineAttributes.ADJDEF);

                    defense = (int)(armorDefense + ((level + (stamina / 2) + dexterity) / 2.8f) - 4 + level * 2);
                }

                defense = Math.Max(defense, 0);
            }

            damages -= (int)(defense * GetDefenseMultiplier());
            damages = Math.Max(0, damages);

            if (damages > 0)
            {
                float blockFactor = GetDefenderBlockFactor();

                if (blockFactor < 1f)
                {
                    attackResult.Flags |= AttackFlags.AF_BLOCKING;
                    attackResult.Damages = (int)(attackResult.Damages * blockFactor);
                }
            }
            else
            {
                flags &= ~AttackFlags.AF_CRITICAL;
                flags &= ~AttackFlags.AF_FLYING;
            }

            return AttackResult.Success(damages, flags);
        }
    }
}
