using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;

namespace Rhisis.Game.Entities
{
    /// <summary>
    /// Describes the map item entity.
    /// </summary>
    public interface IMapItem : IWorldObject
    {
        /// <summary>
        /// Gets the attached item instance.
        /// </summary>
        IItem Item { get; }

        /// <summary>
        /// Gets the item type.
        /// </summary>
        MapItemType ItemType { get; }

        /// <summary>
        /// Gets or sets the item owner.
        /// </summary>
        IWorldObject Owner { get; set; }

        /// <summary>
        /// Gets the item respawn region.
        /// </summary>
        /// <remarks>
        /// Only available when its a permanent map item.
        /// </remarks>
        IMapRespawnRegion RespawnRegion { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the map item has an owner.
        /// </summary>
        bool HasOwner { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the map item is temporary or not.
        /// </summary>
        bool IsTemporary { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the map item is gold.
        /// </summary>
        bool IsGold { get; }

        /// <summary>
        /// Gets or sets the map item owner ship time.
        /// </summary>
        long OwnershipTime { get; set; }

        /// <summary>
        /// Gets or sets the map item despawn time.
        /// </summary>
        long DespawnTime { get; set; }

        /// <summary>
        /// Gets or sets the map item respawn time.
        /// </summary>
        /// <remarks>
        /// Only available if its a permanent map item.
        /// </remarks>
        long RespawnTime { get; set; }
    }
}
