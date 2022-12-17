using Rhisis.Abstractions.Entities;
using Rhisis.Protocol;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers.Friends;

[Handler]
public class AddFriendCancelHandler
{
    [HandlerAction(PacketType.ADDFRIENDCANCEL)]
    public void OnExecute(IPlayer player)
    {
    }
}
