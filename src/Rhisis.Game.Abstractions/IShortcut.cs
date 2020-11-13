using Rhisis.Core.Common;
using Rhisis.Game.Abstractions.Protocol;

namespace Rhisis.Game.Abstractions
{
    public interface IShortcut : IPacketSerializer
    {
        /// <summary>
        /// Gets the shortcut slot in its taskbar container.
        /// </summary>
        int Slot { get; }

        /// <summary>
        /// Gets the shortcut type.
        /// </summary>
        ShortcutType Type { get; }

        /// <summary>
        /// Gets the shortcut object type.
        /// </summary>
        ShortcutObjectType ObjectType { get; }

        /// <summary>
        /// Gets the shortcut item index in the inventory.
        /// </summary>
        /// <remarks>
        /// Only available when <see cref="Type"/> is a <see cref="ShortcutType.Item"/>.
        /// </remarks>
        int? ItemIndex { get; }

        /// <summary>
        /// Gets the shortcut object index in the taskbar container.
        /// </summary>
        uint ObjectIndex { get; }

        /// <summary>
        /// Gets the shortcut user id.
        /// </summary>
        /// <remarks>
        /// This doesn't seem to be used.
        /// </remarks>
        uint UserId { get; }

        /// <summary>
        /// Gets the shortcut additionnal data.
        /// </summary>
        /// <remarks>
        /// This seems to be used in official files to store additionnal data.
        /// Not used for now.
        /// </remarks>
        uint ObjectData { get; }

        /// <summary>
        /// Gets the shortcut text.
        /// </summary>
        /// <remarks>
        /// Only available when <see cref="Type"/> is a <see cref="ShortcutType.Chat"/>.
        /// </remarks>
        string Text { get; }
    }
}
