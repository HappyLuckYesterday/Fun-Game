using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers.Friends
{
    [Handler]
    public class AddFriendCancelHandler
    {
        [HandlerAction(PacketType.ADDFRIENDCANCEL)]
        public void OnExecute(IPlayer player)
        {
        }
    }
}
