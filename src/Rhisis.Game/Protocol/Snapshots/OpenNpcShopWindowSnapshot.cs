using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using System.Collections.Generic;

namespace Rhisis.Network.Snapshots
{
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
}
