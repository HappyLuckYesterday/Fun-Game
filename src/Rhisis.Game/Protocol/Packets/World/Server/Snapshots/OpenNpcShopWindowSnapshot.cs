using Rhisis.Game.Entities;
using Rhisis.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class OpenNpcShopWindowSnapshot : FFSnapshot
{
    public OpenNpcShopWindowSnapshot(Npc npc, IEnumerable<ItemContainer> npcShopTabs)
        : base(SnapshotType.OPENSHOPWND, npc.ObjectId)
    {
        foreach (ItemContainer shopTab in npcShopTabs)
        {
            shopTab.Serialize(this);
        }
    }
}
