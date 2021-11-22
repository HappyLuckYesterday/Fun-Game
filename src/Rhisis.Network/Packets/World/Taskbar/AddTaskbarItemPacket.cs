using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Taskbar
{
    public class AddTaskbarItemPacket : AddTaskbarAppletPacket
    {
        /// <summary>
        /// Gets the slot level index.
        /// </summary>
        public int SlotLevelIndex { get; private set; }

        /// <inheritdoc />
        public override void Deserialize(ILitePacketStream packet)
        {
            SlotLevelIndex = packet.Read<byte>();
            base.Deserialize(packet);
        }
    }
}