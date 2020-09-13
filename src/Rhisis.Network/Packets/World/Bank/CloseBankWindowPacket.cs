using System;
using Rhisis.Game.Abstractions.Protocol;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    public class CloseBankWindowPacket : IPacketDeserializer
    {
        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            throw new NotImplementedException();
        }
    }
}