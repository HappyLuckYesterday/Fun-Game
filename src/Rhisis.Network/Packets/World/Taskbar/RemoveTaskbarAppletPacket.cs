using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Taskbar
{
    public class RemoveTaskbarAppletPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot index from the applet taskbar to be removed.
        /// </summary>
        public int SlotIndex { get; private set; }

        /// <inheritdoc />
        public virtual void Deserialize(ILitePacketStream packet)
        {
            SlotIndex = packet.Read<byte>();
        }
    }
}