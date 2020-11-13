using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Network.Snapshots;

namespace Rhisis.Game.Features.Chat
{
    [Injectable]
    public class Chat : GameFeature, IChat
    {
        private readonly IWorldObject _worldObject;

        public Chat(IWorldObject worldObject)
        {
            _worldObject = worldObject;
        }

        public void Speak(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                // TODO: maybe check for banned words before sending the packet back to the client.
                // Replace the banned words with stars (*)
                using (var chatSnapshot = new ChatSnapshot(_worldObject, text))
                {
                    SendPacketToVisible(_worldObject, chatSnapshot, sendToPlayer: true);
                }
            }
        }

        public void Shout(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
