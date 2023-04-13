using Rhisis.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using Rhisis.Protocol;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class AddObjectSnapshot : FFSnapshot
{
    public enum PlayerAddObjMethodType
    {
        All,
        ExcludeItems
    }

    public AddObjectSnapshot(WorldObject worldObject, PlayerAddObjMethodType playerMode = PlayerAddObjMethodType.All)
        : base(SnapshotType.ADD_OBJ, worldObject.ObjectId)
    {
        WriteByte((byte)worldObject.Type);
        WriteInt32(worldObject.ModelId);
        WriteByte((byte)worldObject.Type);
        WriteInt32(worldObject.ModelId);
        WriteInt16(worldObject.Size);
        WriteSingle(worldObject.Position.X);
        WriteSingle(worldObject.Position.Y);
        WriteSingle(worldObject.Position.Z);
        WriteInt16((short)(worldObject.RotationAngle * 10));
        WriteUInt32(worldObject.ObjectId);

        switch (worldObject)
        {
            case Player player:
                {
                    WriteInt16(0); // m_dwMotion
                    WriteByte(1); // m_bPlayer
                    WriteInt32(player.Health.Hp); // HP
                    WriteInt32((int)player.ObjectState); // moving flags
                    WriteInt32((int)player.ObjectStateFlags); // motion flags
                    WriteByte(1); // m_dwBelligerence

                    WriteInt32(-1); // m_dwMoverSfxId

                    WriteString(player.Name);
                    WriteByte((byte)player.Appearence.Gender);
                    WriteByte((byte)player.Appearence.SkinSetId);
                    WriteByte((byte)player.Appearence.HairId);
                    WriteInt32(player.Appearence.HairColor);
                    WriteByte((byte)player.Appearence.FaceId);
                    WriteInt32(player.Id);
                    WriteByte((byte)player.Job.Id);

                    WriteInt16((short)player.Statistics.Strength);
                    WriteInt16((short)player.Statistics.Stamina);
                    WriteInt16((short)player.Statistics.Dexterity);
                    WriteInt16((short)player.Statistics.Intelligence);

                    WriteInt16((short)player.Level); // Level
                    WriteInt32(-1); // Fuel
                    WriteInt32(0); // Actuel fuel

                    // Guilds

                    WriteByte(0); // have guild or not
                    WriteInt32(0); // guild cloak

                    // Party

                    WriteByte(0); // have party or not

                    WriteByte((byte)player.Authority); // authority
                    WriteUInt32((uint)player.Mode); // mode
                    WriteInt32((int)player.StateMode); // state mode
                    WriteInt32(0); // item used ??
                    WriteInt32(0); // last pk time.
                    WriteInt32(0); // karma
                    WriteInt32(0); // pk propensity
                    WriteInt32(0); // pk exp
                    WriteInt32(0); // fame
                    WriteByte(0); // duel
                    WriteInt32(-1); // titles

                    // Serialize visible items
                    IEnumerable<Item> equipedItems = player.GetEquipedItems();

                    foreach (Item item in equipedItems)
                    {
                        WriteInt32(item?.Refines ?? 0);
                    }

                    WriteInt32(0); // guild war state

                    for (int i = 0; i < 26; i++)
                    {
                        WriteInt32(0);
                    }

                    if (playerMode == PlayerAddObjMethodType.All)
                    {
                        WriteInt16((short)player.Health.Mp);
                        WriteInt16((short)player.Health.Fp);
                        WriteInt32(0); // tutorial state
                        WriteInt32(0); // fly experience
                        WriteInt32(player.Gold.Amount); // Gold
                        WriteInt64(player.Experience.Amount); // exp
                        WriteInt32(0); // skill level
                        WriteInt32((int)player.SkillPoints); // skill points
                        WriteInt64(0); // death exp
                        WriteInt32(0); // death level

                        for (var i = 0; i < 32; ++i)
                        {
                            WriteInt32(0); // job in each level
                        }

                        WriteInt32(0); // marking world id
                        WriteSingle(player.Position.X);
                        WriteSingle(player.Position.Y);
                        WriteSingle(player.Position.Z);

                        // Quest diary
                        player.QuestDiary.Serialize(this);

                        WriteInt32(0); // murderer id
                        WriteInt16((short)player.AvailablePoints); // stat points
                        WriteInt16(0); // always 0

                        // Item mask
                        foreach (Item item in equipedItems)
                        {
                            WriteInt32(item?.Id ?? -1);
                        }

                        player.Skills.Serialize(this);

                        WriteByte(0); // cheer point
                        WriteInt32(0); // next cheer point ?

                        // Bank
                        WriteByte((byte)player.Slot);
                        for (var i = 0; i < 3; ++i)
                        {
                            WriteInt32(0); // gold
                        }

                        for (var i = 0; i < 3; ++i)
                        {
                            WriteInt32(0); // player bank ?
                        }

                        WriteInt32(1); // ar << m_nPlusMaxHitPoint
                        WriteByte(0); // ar << m_nAttackResistLeft			
                        WriteByte(0); // ar << m_nAttackResistRight			
                        WriteByte(0); // ar << m_nDefenseResist
                        WriteInt64(0); // ar << m_nAngelExp
                        WriteInt32(0); // ar << m_nAngelLevel

                        // Inventory
                        player.Inventory.Serialize(this);

                        // Bank
                        for (var i = 0; i < 3; ++i)
                        {
                            for (var j = 0; j < 0x2A; ++j)
                            {
                                WriteInt32(j);
                            }

                            WriteByte(0); // count
                            for (var j = 0; j < 0x2A; ++j)
                            {
                                WriteInt32(j);
                            }
                        }

                        WriteInt32(-1); // pet id

                        // Bag
                        WriteByte(0);
                        WriteByte(0);
                        WriteByte(0);

                        WriteInt32(0); // muted

                        // Honor titles
                        for (var i = 0; i < 150; ++i)
                        {
                            WriteInt32(0);
                        }

                        WriteInt32(0); // id campus
                        WriteInt32(0); // campus points
                    }
                    else if (playerMode == PlayerAddObjMethodType.ExcludeItems)
                    {
                        WriteInt32(0); // Player vendor name
                        WriteByte((byte)equipedItems.Count(x => x != null));

                        foreach (Item item in equipedItems)
                        {
                            if (item != null)
                            {
                                WriteByte((byte)item.Properties.Parts);
                                WriteInt16((short)item.Id);
                                WriteByte(0); // item flag
                            }
                        }

                        WriteInt32(-1); // pet
                        WriteInt32(0);
                    }

                    // buffs
                    WriteInt32(0);
                    //player.Buffs.Serialize(this);
                }
                break;
            case Monster monster:
                {
                    WriteInt16(1);
                    WriteByte(0);
                    WriteInt32(monster.Health.Hp);
                    WriteInt32(1);
                    WriteInt32(0);
                    WriteByte((byte)monster.Properties.Belligerence);
                    WriteInt32(-1);

                    WriteByte(0);
                    WriteInt32(-1);
                    WriteByte(0);
                    WriteString(monster.Name.TakeCharacters(31));
                    WriteByte(0);
                    WriteByte(0);
                    WriteByte(0);
                    WriteByte(0);
                    WriteInt32(0);
                    WriteSingle(monster.SpeedFactor); // speed factor
                    WriteInt32(0); // Buff count
                }
                break;
            case Npc npc:
                {
                    WriteInt16(1);
                    WriteByte(0);
                    WriteInt32(1);
                    WriteInt32(1);
                    WriteInt32(0);
                    WriteByte(1);
                    WriteInt32(-1);

                    WriteByte(0); // Npc hair id
                    WriteInt32(0); // Npc hair color
                    WriteByte(0); // Npc Face Id
                    WriteString(npc.Name.TakeCharacters(31));
                    WriteByte(0); // item equiped count
                    WriteByte(0);
                    WriteByte(0);
                    WriteByte(0);
                    WriteInt32(0);
                    WriteSingle(1); // speed factor
                    WriteInt32(0); // Buff count
                }
                break;
            case MapItemObject mapItem:
                mapItem.Serialize(this);
                break;
            default: 
                throw new NotImplementedException();
        }
    }
}
