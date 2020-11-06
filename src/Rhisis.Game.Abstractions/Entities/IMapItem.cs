using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;

namespace Rhisis.Game.Entities
{
    public interface IMapItem : IWorldObject
    {
        IItem Item { get; }

        IWorldObject Owner { get; set; }

        IMapRespawnRegion RespawnRegion { get; }

        bool HasOwner { get; }

        bool IsTemporary { get; }

        bool IsGold { get; }

        long OwnershipTime { get; set; }

        long DespawnTime { get; set; }

        long RespawnTime { get; set; }
    }
}
