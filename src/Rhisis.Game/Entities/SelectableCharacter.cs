using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;
using System.Collections.Generic;

namespace Rhisis.Game.Entities;

public class SelectableCharacter : ISelectableCharacter
{
    public int Id { get; set; }

    public string Name { get; set; }

    public GenderType Gender { get; set; }

    public int Level { get; set; }

    public int Slot { get; set; }

    public int MapId { get; set; }

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public float PositionZ { get; set; }

    public int SkinSetId { get; set; }

    public int HairId { get; set; }

    public uint HairColor { get; set; }

    public int FaceId { get; set; }

    public int JobId { get; set; }

    public int Strength { get; set; }

    public int Stamina { get; set; }

    public int Intelligence { get; set; }

    public int Dexterity { get; set; }

    public IEnumerable<int> EquipedItems { get; set; }
}
