using Rhisis.Abstractions;
using Rhisis.Abstractions.Features;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Game.Features;

public class Taskbar : ITaskbar
{
    public int ActionPoints { get; set; } = 100;

    public ITaskbarContainer<IShortcut> Applets { get; }

    public IMultipleTaskbarContainer<IShortcut> Items { get; }

    public ITaskbarContainer<ISkill> ActionSlot { get; }

    public Taskbar()
    {
        Applets = new TaskbarContainer<IShortcut>(GameConstants.MaxTaskbarApplets);
        Items = new MultipleTaskbarContainer<IShortcut>(GameConstants.MaxTaskbarItemLevels, GameConstants.MaxTaskbarItems);
    }

    public void Serialize(IFFPacket packet)
    {
        Applets.Serialize(packet);
        Items.Serialize(packet);
        packet.WriteInt32(0);
        //ActionSlot.Serialize(packet);
        packet.WriteInt32(ActionPoints);
    }
}
