using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Game.Protocol.Packets;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.WorldServer.Handlers.Friends.Messages
{
    [Handler]
    public class PlayerMessengerSayMessage
    {
        private readonly IWorldServer _server;

        public PlayerMessengerSayMessage(IWorldServer server)
        {
            _server = server;
        }

        [HandlerAction(typeof(PlayerMessengerMessage))]
        public void OnPlayerMessengerMessage(PlayerMessengerMessage message)
        {
            IPlayer receiverPlayer = _server.GetPlayerEntityByCharacterId((uint)message.DestinationId);

            if (receiverPlayer is null)
            {
                return;
            }

            using var sayPacket = new SayPacket(message.FromId, message.FromName, 
                message.DestinationId, receiverPlayer.Name, 
                message.Message);
            receiverPlayer.Send(sayPacket);
        }
    }
}
