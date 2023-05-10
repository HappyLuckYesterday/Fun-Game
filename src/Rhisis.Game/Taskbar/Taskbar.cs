using Rhisis.Protocol;

namespace Rhisis.Game.TaskbarPlayer;

public class Taskbar : IPacketSerializer
{
    public int ActionPoints { get; set; } = 100;

    public TaskbarContainer<Shortcut> Applets { get; }

    public MultipleTaskbarContainer<Shortcut> Items { get; }

    // public TaskbarContainer<Skill> ActionSlot { get; }

    public Taskbar()
    {
        Applets = new TaskbarContainer<Shortcut>(18);
        Items = new MultipleTaskbarContainer<Shortcut>(8, 9);
    }

    public void Serialize(FFPacket packet)
    {
        Applets.Serialize(packet);
        Items.Serialize(packet);
        packet.WriteInt32(0);
        //ActionSlot.Serialize(packet);
        packet.WriteInt32(ActionPoints);
    }
}
