using Rhisis.Abstractions.Entities;
using Rhisis.Protocol;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers.NpcShop;

[Handler]
public class CloseNpcShopHandler
{
    [HandlerAction(PacketType.CLOSESHOPWND)]
    public void Execute(IPlayer player)
    {
        player.CurrentNpcShopName = null;
    }
}
