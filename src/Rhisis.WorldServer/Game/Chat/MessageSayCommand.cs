using Rhisis.Game.Abstractions.Caching;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Abstractions.Messaging;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Messages;
using Rhisis.Game.Protocol.Packets;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/say", AuthorityType.Player)]
    public class MessageSayCommand : IChatCommand
    {
        private readonly IMessaging _messaging;
        private readonly IPlayerCache _playerCache;

        public ChatParameterParsingType ParsingType => ChatParameterParsingType.PlainText;

        public MessageSayCommand(IMessaging messaging, IPlayerCache playerCache)
        {
            _messaging = messaging;
            _playerCache = playerCache;
        }

        public void Execute(IPlayer player, object[] parameters)
        {
            // TODO: check if player is muted.
            if (parameters.Length < 1)
            {
                throw new InvalidOperationException("Failed to use /say command. To few arguments.");
            }

            string parameter = parameters.ElementAt(0).ToString();
            string destinationPlayerName = parameter.Split(' ').ElementAt(0);
            string message = parameter.Replace(destinationPlayerName, "").Trim();

            CachedPlayer destinationPlayer = _playerCache.GetCachedPlayer(destinationPlayerName.Trim('"'));

            if (destinationPlayer is null)
            {
                throw new InvalidOperationException($"Cannot find player with name: {destinationPlayerName}");
            }

            if (!destinationPlayer.IsOnline)
            {
                throw new InvalidOperationException($"Player {destinationPlayerName} is not connected.");
            }

            using var sayPacket = new SayPacket(player.CharacterId, player.Name,
                destinationPlayer.Id, destinationPlayer.Name,
                message);
            player.Send(sayPacket);
            _messaging.Publish(new PlayerMessengerMessage(player.CharacterId, player.Name, destinationPlayer.Id, message));
        }
    }
}