using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Network.Snapshots;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public class ChatSystem : GameFeature, IChatSystem
    {
        public void Speak(IWorldObject worldObject, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                // TODO: maybe check for banned words before sending the packet back to the client.
                // Replace the banned words with stars (*)
                using (var chatSnapshot = new ChatSnapshot(worldObject, text))
                {
                    SendPacketToVisible(worldObject, chatSnapshot, sendToPlayer: true);
                }
            }
        }

        public void Shout(IWorldObject worldObjet, string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
