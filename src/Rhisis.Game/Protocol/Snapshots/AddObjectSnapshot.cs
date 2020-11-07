using Rhisis.Core.Extensions;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Network.Snapshots
{
    public class AddObjectSnapshot : FFSnapshot
    {
        public enum PlayerAddObjMethodType
        {
            All,
            ExcludeItems
        }

        public AddObjectSnapshot(IWorldObject worldObject, PlayerAddObjMethodType playerMode = PlayerAddObjMethodType.All)
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
                        Write((int)player.StateMode); // state mode
                        Write(0); // item used ??
                        Write(0); // last pk time.
                        Write(0); // karma
                        Write(0); // pk propensity
                        Write(0); // pk exp
                        Write(0); // fame
                        Write<byte>(0); // duel
                        Write(-1); // titles

                        // Serialize visible items
                        IEnumerable<IItem> equipedItems = player.Inventory.GetEquipedItems();

                        foreach (var item in equipedItems)
                        {
                            Write(item?.Refines ?? 0);
                        }

                        Write(0); // guild war state

                        for (int i = 0; i < 26; i++)
                        {
                            Write(0);
                        }

                        if (playerMode == PlayerAddObjMethodType.All)
                        {
                            Write((short)player.Health.Mp);
                            Write((short)player.Health.Fp);
                            Write(0); // tutorial state
                            Write(0); // fly experience
                            Write(player.Gold.Amount); // Gold
                            Write(player.Experience.Amount); // exp
                            Write(0); // skill level
                            Write((int)player.SkillTree.SkillPoints); // skill points
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
                            player.Quests.Serialize(this);

                            Write(0); // murderer id
                            Write((short)player.Statistics.AvailablePoints); // stat points
                            Write<short>(0); // always 0

                            // Item mask
                            foreach (var item in equipedItems)
                            {
                                Write(item?.Id ?? -1);
                            }

                            player.SkillTree.Serialize(this);

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
                        }
                        else if (playerMode == PlayerAddObjMethodType.ExcludeItems)
                        {
                            Write(0); // Player vendor name
                            Write((byte)equipedItems.Count(x => x != null));

                            foreach (IItem item in equipedItems)
                            {
                                if (item != null)
                                {
                                    Write((byte)item.Data.Parts);
                                    Write((short)item.Id);
                                    Write<byte>(0); // item flag
                                }
                            }

                            Write(-1); // pet
                            Write(0);
                        }

                        // buffs
                        Write(0);
                    }
                    break;
                case IMonster monster:
                    {
                        Write<short>(1);
                        Write<byte>(0);
                        Write(monster.Health.Hp);
                        Write(1);
                        Write(0);
                        Write((byte)monster.Data.Belligerence);
                        Write(-1);

                        Write((byte)0);
                        Write(-1);
                        Write((byte)0);
                        Write(monster.Name.TakeCharacters(31));
                        Write((byte)0);
                        Write((byte)0);
                        Write((byte)0);
                        Write((byte)0);
                        Write(0);
                        Write(monster.SpeedFactor); // speed factor
                        Write(0); // Buff count
                    }
                    break;
                case INpc npc:
                    {
                        Write<short>(1);
                        Write<byte>(0);
                        Write(1);
                        Write(1);
                        Write(0);
                        Write<byte>(1);
                        Write(-1);

                        Write<byte>(0); // Npc hair id
                        Write(0); // Npc hair color
                        Write<byte>(0); // Npc Face Id
                        Write(npc.Key.TakeCharacters(31));
                        Write<byte>(0); // item equiped count
                        Write<byte>(0);
                        Write<byte>(0);
                        Write<byte>(0);
                        Write(0);
                        Write<float>(1); // speed factor
                        Write(0); // Buff count
                    }
                    break;
                case IMapItem mapItem:
                    mapItem.Item.Serialize(this, -1);
                    break;
            }
        }
    }
}
