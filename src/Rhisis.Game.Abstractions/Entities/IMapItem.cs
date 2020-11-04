using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Entities
{
    public interface IMapItem : IWorldObject
    {
        IItem Item { get; }

        IWorldObject Owner { get; set; }

        bool HasOwner { get; }

        bool IsTemporary { get; }

        bool IsGold { get; }

        long OwnershipTime { get; set; }

        long DespawnTime { get; set; }

        long RespawnTime { get; set; }
    }
}
