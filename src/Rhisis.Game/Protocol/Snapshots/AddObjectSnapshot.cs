using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;

namespace Rhisis.Network.Snapshots
{
    public class AddObjectSnapshot : FFSnapshot
    {
        public AddObjectSnapshot(IWorldObject worldObject)
            : base(SnapshotType.ADD_OBJ, worldObject.Id)
        {
            Write((byte)worldObject.Type);
            Write(worldObject.ModelId);
            Write((byte)worldObject.Type);
            Write(worldObject.ModelId);
            Write(worldObject.Size);
            Write(worldObject.Position.X);
            Write(worldObject.Position.Y);
            Write(worldObject.Position.Z);
            Write((short)(worldObject.Angle * 10));
            Write(worldObject.Id);

            switch (worldObject)
            {
                case IPlayer player:
                    {
                        Write<short>(0); // m_dwMotion
                        Write<byte>(1); // m_bPlayer
                        Write(player.Health.Hp); // HP
                        Write((int)player.ObjectState); // moving flags
                        Write((int)player.ObjectStateFlags); // motion flags
                        Write<byte>(1); // m_dwBelligerence

                        Write(-1); // m_dwMoverSfxId

                        Write(player.Name);
                        Write((byte)player.Appearence.Gender);
                        Write((byte)player.Appearence.SkinSetId);
                        Write((byte)player.Appearence.HairId);
                        Write(player.Appearence.HairColor);
                        Write((byte)player.Appearence.FaceId);
                        Write(player.CharacterId);
                        Write((byte)player.Job.Id);

                        Write((short)player.Statistics.Strength);
                        Write((short)player.Statistics.Stamina);
                        Write((short)player.Statistics.Dexterity);
                        Write((short)player.Statistics.Intelligence);

                        Write((short)player.Level); // Level
                        Write(-1); // Fuel
                        Write(0); // Actuel fuel

                        // Guilds

                        Write<byte>(0); // have guild or not
                        Write(0); // guild cloak

                        // Party

                        Write<byte>(0); // have party or not

                        Write((byte)player.Authority); // authority
                        Write((uint)player.Mode); // mode
                        Write(0); // state mode
                        Write(0); // item used ??
                        Write(0); // last pk time.
                        Write(0); // karma
                        Write(0); // pk propensity
                        Write(0); // pk exp
                        Write(0); // fame
                        Write<byte>(0); // duel
                        Write(-1); // titles

                        // Serialize visible items
                        IEnumerable<IItem> equippedItems = player.Inventory.GetEquipedItems();

                        foreach (var item in equippedItems)
                        {
                            Write(item?.Refines ?? 0);
                        }

                        Write(0); // guild war state

                        for (int i = 0; i < 26; i++)
                        {
                            Write(0);
                        }

                        Write((short)player.Health.Mp);
                        Write((short)player.Health.Fp);
                        Write(0); // tutorial state
                        Write(0); // fly experience
                        Write(player.Gold); // Gold
                        Write(player.Experience); // exp
                        Write(0); // skill level
                        Write(0/*(int)player.SkillTree.SkillPoints*/); // skill points
                        Write<long>(0); // death exp
                        Write(0); // death level

                        for (var i = 0; i < 32; ++i)
                        {
                            Write(0); // job in each level
                        }

                        Write(0); // marking world id
                        Write(player.Position.X);
                        Write(player.Position.Y);
                        Write(player.Position.Z);

                        // Quest diary
                        Write((byte)0);
                        Write((byte)0);
                        Write((byte)0);

                        Write(0); // murderer id
                        Write((short)player.Statistics.AvailablePoints); // stat points
                        Write<short>(0); // always 0

                        // Item mask
                        foreach (var item in equippedItems)
                        {
                            Write(item?.Id ?? -1);
                        }

                        for (int i = 0; i < (int)DefineJob.JobMax.MAX_SKILLS; i++)
                        {
                            Write(-1);
                            Write(0);
                        }

                        Write<byte>(0); // cheer point
                        Write(0); // next cheer point ?

                        // Bank
                        Write((byte)player.Slot);
                        for (var i = 0; i < 3; ++i)
                            Write(0); // gold
                        for (var i = 0; i < 3; ++i)
                            Write(0); // player bank ?

                        Write(1); // ar << m_nPlusMaxHitPoint
                        Write<byte>(0); // ar << m_nAttackResistLeft			
                        Write<byte>(0); // ar << m_nAttackResistRight			
                        Write<byte>(0); // ar << m_nDefenseResist
                        Write<long>(0); // ar << m_nAngelExp
                        Write(0); // ar << m_nAngelLevel

                        // Inventory
                        player.Inventory.Serialize(this);

                        // Bank
                        for (var i = 0; i < 3; ++i)
                        {
                            for (var j = 0; j < 0x2A; ++j)
                                Write(j);
                            Write<byte>(0); // count
                            for (var j = 0; j < 0x2A; ++j)
                                Write(j);
                        }

                        Write(-1); // pet id

                        // Bag
                        Write<byte>(0);
                        Write<byte>(0);
                        Write<byte>(0);

                        Write(0); // muted

                        // Honor titles
                        for (var i = 0; i < 150; ++i)
                            Write(0);

                        Write(0); // id campus
                        Write(0); // campus points

                        // buffs
                        Write(0);
                    }
                    break;
                case IMonster monster:
                    {
                        throw new NotImplementedException(monster.Name);
                    }
                    break;
                case INpc npc:
                    {
                        throw new NotImplementedException(npc.Name);
                    }
                    break;
            }
        }
    }
}
