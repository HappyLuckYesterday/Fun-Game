using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Taskbar
{
    public class AddTaskbarItemPacket : AddTaskbarAppletPacket
    {
        /// <summary>
        /// Gets the slot level index.
        /// </summary>
        public int SlotLevelIndex { get; private set; }

        /// <inheritdoc />
        public override void Deserialize(IFFPacket packet)
        {
            SlotLevelIndex = packet.ReadByte();
            base.Deserialize(packet);
        }
    }
}