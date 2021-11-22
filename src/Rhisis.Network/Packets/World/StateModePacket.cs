using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
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
        public void Deserialize(ILitePacketStream packet)
        {
            StateMode = (StateMode)packet.Read<int>();
            Flag = (StateModeBaseMotion)packet.Read<byte>();
        }
    }
}
