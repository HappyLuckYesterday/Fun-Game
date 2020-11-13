using System;
using Rhisis.Game.Abstractions.Protocol;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    public class ConfirmBankPacket : IPacketDeserializer
    {
        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            throw new NotImplementedException();
        }
    }
}