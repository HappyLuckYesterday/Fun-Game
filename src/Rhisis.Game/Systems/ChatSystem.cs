using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Network.Snapshots;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public class ChatSystem : IChatSystem
    {
        public void Speak(IWorldObject worldObject, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                using (var chatSnapshot = new ChatSnapshot(worldObject, text))
                {
                    IEnumerable<IPlayer> visiblePlayers = worldObject.VisibleObjects.OfType<IPlayer>();

                    foreach (IPlayer player in visiblePlayers)
                    {
                        player.Connection.Send(chatSnapshot);
                    }

                    if (worldObject is IPlayer currentPlayer)
                    {
                        currentPlayer.Connection.Send(chatSnapshot);
                    }
                }
            }
        }

        public void Shout(IWorldObject worldObjet, string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
