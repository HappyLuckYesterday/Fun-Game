using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Features;
using System.Collections.Generic;

namespace Rhisis.Protocol.Snapshots;

public class OpenNpcShopWindowSnapshot : FFSnapshot
{
    public OpenNpcShopWindowSnapshot(INpc npc, IEnumerable<IItemContainer> npcShopTabs)
        : base(SnapshotType.OPENSHOPWND, npc.Id)
    {
        foreach (IItemContainer shopTab in npcShopTabs)
        {
            shopTab.Serialize(this);
        }
    }
}
