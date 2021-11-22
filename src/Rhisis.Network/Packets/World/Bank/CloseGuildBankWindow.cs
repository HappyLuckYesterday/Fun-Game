using System;
using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Bank
{
    public class CloseGuildBankWindow : IPacketDeserializer
    {
        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            throw new NotImplementedException();
        }
    }
}