using Rhisis.Game.Common;
using System.Collections.Generic;

namespace Rhisis.Abstractions.Entities
{
    /// <summary>
    /// Describes a character on the character selection screen.
    /// </summary>
    public interface ISelectableCharacter
    {
        /// <summary>
        /// Gets the character id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the character name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the character gender.
        /// </summary>
        public GenderType Gender { get; }

        /// <summary>
        /// Gets the character level.
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// Gets the character slot on character selection screen.
        /// </summary>
        public int Slot { get; }

        /// <summary>
        /// Gets the character map id.
        /// </summary>
        public int MapId { get; }

        /// <summary>
        /// Gets the character X position.
        /// </summary>
        public float PositionX { get; }

        /// <summary>
        /// Gets the character Y position.
        /// </summary>
        public float PositionY { get; }

        /// <summary>
        /// Gets the character Z position.
        /// </summary>
        public float PositionZ { get; }

        /// <summary>
        /// Gets the character skin set id.
        /// </summary>
        public int SkinSetId { get; }

        /// <summary>
        /// Gets the character hair mesh id.
        /// </summary>
        public int HairId { get; }

        /// <summary>
        /// Gets the character hair color.
        /// </summary>
        public uint HairColor { get; }

        /// <summary>
        /// Gets the character face mesh id.
        /// </summary>
        public int FaceId { get; }

        /// <summary>
        /// Gets the character job id.
        /// </summary>
        public int JobId { get; }

        /// <summary>
        /// Gets the character strength.
        /// </summary>
        public int Strength { get; }

        /// <summary>
        /// Gets the character stamina.
        /// </summary>
        public int Stamina { get; }

        /// <summary>
        /// Gets the character intelligence.
        /// </summary>
        public int Intelligence { get; }

        /// <summary>
        /// Gets the character dexterity.
        /// </summary>
        public int Dexterity { get; }

        /// <summary>
        /// Gets the character equiped items ids.
        /// </summary>
        public IEnumerable<int> EquipedItems { get; }
    }
}
