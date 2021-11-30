using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World.Taskbar
{
    public class RemoveTaskbarAppletPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot index from the applet taskbar to be removed.
        /// </summary>
        public int SlotIndex { get; private set; }

        /// <inheritdoc />
        public virtual void Deserialize(IFFPacket packet)
        {
            SlotIndex = packet.Read<byte>();
        }
    }
}