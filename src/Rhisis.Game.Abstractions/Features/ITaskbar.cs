using Rhisis.Game.Abstractions.Protocol;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage the player taskbar.
    /// </summary>
    public interface ITaskbar : IPacketSerializer
    {
        /// <summary>
        /// Gets the player action points.
        /// </summary>
        int ActionPoints { get; }

        /// <summary>
        /// Gets the shortcut applets container.
        /// </summary>
        ITaskbarContainer<IShortcut> Applets { get; }

        /// <summary>
        /// Gets the multi level taskbar containers.
        /// </summary>
        /// <remarks>
        /// Can host applets, skills and items.
        /// </remarks>
        IMultipleTaskbarContainer<IShortcut> Items { get; }

        /// <summary>
        /// Gets the action slot container.
        /// </summary>
        ITaskbarContainer<ISkill> ActionSlot { get; }
    }
}
