using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Features;
using Sylver.Network.Data;

namespace Rhisis.Game.Features
{
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

        public void Serialize(INetPacketStream packet)
        {
            Applets.Serialize(packet);
            Items.Serialize(packet);
            packet.Write(0);
            //ActionSlot.Serialize(packet);
            packet.Write(ActionPoints);
        }
    }
}
