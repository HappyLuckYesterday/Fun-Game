using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IChatSystem
    {
        /// <summary>
        /// Makes the given world object speak the given message to all visible objects.
        /// </summary>
        /// <param name="worldObject">Current world object.</param>
        /// <param name="text">Text to be sent as normal chat message.</param>
        void Speak(IWorldObject worldObject, string text);

        /// <summary>
        /// Makes the given world object shout the given message to all visible objects.
        /// </summary>
        /// <param name="worldObject">Current world object.</param>
        /// <param name="text">Text to be sent as shouting chat message.</param>
        void Shout(IWorldObject worldObjet, string text);
    }
}
