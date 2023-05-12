using Rhisis.Protocol;

namespace Rhisis.Game;

public class Taskbar : IPacketSerializer
{
    /// <summary>
    /// Gets or sets the action slot points.
    /// </summary>
    public int ActionPoints { get; set; } = 100;

    /// <summary>
    /// Gets the applets taskbar.
    /// </summary>
    public TaskbarContainer<Shortcut> Applets { get; }

    /// <summary>
    /// Gets the items taskbar.
    /// </summary>
    public MultipleTaskbarContainer<Shortcut> Items { get; }

    /// <summary>
    /// Gets the action slot taskbar.
    /// </summary>
    public TaskbarContainer<Skill> ActionSlot { get; }

    /// <summary>
    /// Creates a new <see cref="Taskbar"/> instance.
    /// </summary>
    public Taskbar()
    {
        Applets = new TaskbarContainer<Shortcut>(18);
        Items = new MultipleTaskbarContainer<Shortcut>(8, 9);
        ActionSlot = new TaskbarContainer<Skill>(5);
    }

    /// <summary>
    /// Serializes the taskbar into the packet.
    /// </summary>
    /// <param name="packet">Packet.</param>
    public void Serialize(FFPacket packet)
    {
        Applets.Serialize(packet);
        Items.Serialize(packet);
        ActionSlot.Serialize(packet);
        packet.WriteInt32(ActionPoints);
    }
}
