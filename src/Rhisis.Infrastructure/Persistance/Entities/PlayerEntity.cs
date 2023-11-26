using System;
using System.Collections.Generic;

namespace Rhisis.Infrastructure.Persistance.Entities;

public sealed class PlayerEntity
{
    /// <summary>
    /// Gets or sets the character id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the character account id.
    /// </summary>
    public int AccountId { get; set; }

    /// <summary>
    /// Gets or sets the character name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the character gender.
    /// </summary>
    public byte Gender { get; set; }

    /// <summary>
    /// Gets or sets the character level.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Gets or sets the character level experience progression.
    /// </summary>
    public long Experience { get; set; }

    /// <summary>
    /// Gets or sets the character class.
    /// </summary>
    public int JobId { get; set; }

    /// <summary>
    /// Gets or sets the character gold amount.
    /// </summary>
    public int Gold { get; set; }

    /// <summary>
    /// Gets or sets the character slot.
    /// </summary>
    public byte Slot { get; set; }

    /// <summary>
    /// Gets or sets the character strength.
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// Gets or sets the character stamina.
    /// </summary>
    public int Stamina { get; set; }

    /// <summary>
    /// Gets or sets the character dexterity.
    /// </summary>
    public int Dexterity { get; set; }

    /// <summary>
    /// Gets or sets the character intelligence.
    /// </summary>
    public int Intelligence { get; set; }

    /// <summary>
    /// Gets or sets the character hit points.
    /// </summary>
    public int Hp { get; set; }

    /// <summary>
    /// Gets or sets the character magic points.
    /// </summary>
    public int Mp { get; set; }

    /// <summary>
    /// Gets or sets the character fatigue points.
    /// </summary>
    public int Fp { get; set; }

    /// <summary>
    /// Gets or sets the character skin set id.
    /// </summary>
    public int SkinSetId { get; set; }

    /// <summary>
    /// Gets or sets the character hair id.
    /// </summary>
    public int HairId { get; set; }

    /// <summary>
    /// Gets or sets the character hair color.
    /// </summary>
    public int HairColor { get; set; }

    /// <summary>
    /// Gets or sets the character hair id.
    /// </summary>
    public int FaceId { get; set; }

    /// <summary>
    /// Gets or sets the character map id.
    /// </summary>
    public int MapId { get; set; }

    /// <summary>
    /// Gets or sets character map layer id.
    /// </summary>
    public int MapLayerId { get; set; }

    /// <summary>
    /// Gets or sets the character X position.
    /// </summary>
    public float PosX { get; set; }

    /// <summary>
    /// Gets or sets the character Y position.
    /// </summary>
    public float PosY { get; set; }

    /// <summary>
    /// Gets or sets the character Z position.
    /// </summary>
    public float PosZ { get; set; }

    /// <summary>
    /// Gets or sets the character orientation angle.
    /// </summary>
    public float Angle { get; set; }

    /// <summary>
    /// Gets or sets the character bank code.
    /// </summary>
    public int BankCode { get; set; }

    /// <summary>
    /// Gets or sets the character remaining statistics points.
    /// </summary>
    public int StatPoints { get; set; }

    /// <summary>
    /// Gets or sets the character remaining skill points.
    /// </summary>
    public int SkillPoints { get; set; }

    /// <summary>
    /// Gets or sets the last connection time.
    /// </summary>
    public DateTime LastConnectionTime { get; set; }

    /// <summary>
    /// Gets or sets the play time amount in seconds.
    /// </summary>
    public long PlayTime { get; set; }

    /// <summary>
    /// Gets or sets a flag that indicates if the character is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the player items.
    /// </summary>
    public ICollection<PlayerItemEntity> Items { get; set; } = new HashSet<PlayerItemEntity>();

    /// <summary>
    /// Gets or sets the player skills.
    /// </summary>
    public ICollection<PlayerSkillEntity> Skills { get; set; } = new HashSet<PlayerSkillEntity>();

    /// <summary>
    /// Gets or sets the player skill buffs.
    /// </summary>
    public ICollection<PlayerSkillBuffEntity> Buffs { get; set; } = new HashSet<PlayerSkillBuffEntity>();

    /// <summary>
    /// Gets or sets the player quests.
    /// </summary>
    public ICollection<PlayerQuestEntity> Quests { get; set; } = new HashSet<PlayerQuestEntity>();
}
