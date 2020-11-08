using Rhisis.Game.Abstractions.Protocol;

namespace Rhisis.Game.Abstractions.Features
{
    public interface ITaskbar : IPacketSerializer
    {
        int ActionPoints { get; }

        ITaskbarContainer<IShortcut> Applets { get; }

        IMultipleTaskbarContainer<IShortcut> Items { get; }

        ITaskbarContainer<ISkill> ActionSlot { get; }
    }
}
