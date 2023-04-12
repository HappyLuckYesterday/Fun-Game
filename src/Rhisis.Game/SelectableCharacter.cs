using System.Collections.Generic;
using Rhisis.Game.Common;

namespace Rhisis.Game;

/// <summary>
/// Describes a character on the character selection screen.
/// </summary>
public sealed class SelectableCharacter
{
    /// <summary>
    /// Gets the character id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets the character name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the character gender.
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// Gets the character level.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Gets the character slot on character selection screen.
    /// </summary>
    public int Slot { get; set; }

    /// <summary>
    /// Gets the character map id.
    /// </summary>
    public int MapId { get; set; }

    /// <summary>
    /// Gets the character X position.
    /// </summary>
    public float PositionX { get; set; }

    /// <summary>
    /// Gets the character Y position.
    /// </summary>
    public float PositionY { get; set; }

    /// <summary>
    /// Gets the character Z position.
    /// </summary>
    public float PositionZ { get; set; }

    /// <summary>
    /// Gets the character skin set id.
    /// </summary>
    public int SkinSetId { get; set; }

    /// <summary>
    /// Gets the character hair mesh id.
    /// </summary>
    public int HairId { get; set; }

    /// <summary>
    /// Gets the character hair color.
    /// </summary>
    public uint HairColor { get; set; }

    /// <summary>
    /// Gets the character face mesh id.
    /// </summary>
    public int FaceId { get; set; }

    /// <summary>
    /// Gets the character job id.
    /// </summary>
    public int JobId { get; set; }

    /// <summary>
    /// Gets the character strength.
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// Gets the character stamina.
    /// </summary>
    public int Stamina { get; set; }

    /// <summary>
    /// Gets the character intelligence.
    /// </summary>
    public int Intelligence { get; set; }

    /// <summary>
    /// Gets the character dexterity.
    /// </summary>
    public int Dexterity { get; set; }

    /// <summary>
    /// Gets the character equiped items ids.
    /// </summary>
    public IEnumerable<int> EquipedItems { get; set; }
}

