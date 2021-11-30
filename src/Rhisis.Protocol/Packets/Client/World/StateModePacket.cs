using Rhisis.Game.Common;
using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class StateModePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the client state mode.
        /// </summary>
        public StateMode StateMode { get; private set; }

        /// <summary>
        /// Gets the client state mode flag.
        /// </summary>
        public StateModeBaseMotion Flag { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            StateMode = (StateMode)packet.Read<int>();
            Flag = (StateModeBaseMotion)packet.Read<byte>();
        }
    }
}
