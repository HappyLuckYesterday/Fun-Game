using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Protocol;
using Rhisis.Core.Helpers;
using System;

namespace Rhisis.Protocol
{
    /// <summary>
    /// Represents a FlyFF user connection.
    /// </summary>
    public class FFUserConnection : LiteServerUser
    {
        /// <summary>
        /// Gets the user session id.
        /// </summary>
        public uint SessionId { get; } = RandomHelper.GenerateSessionKey();

        /// <summary>
        /// Gets the connection logger.
        /// </summary>
        protected ILogger Logger { get; private set; }

        protected FFUserConnection(ILogger logger)
        {
            Logger = logger;
        }

        public override void Send(byte[] packetBuffer)
        {
            Logger.LogTrace("Send {0} packet to {1}.", (PacketType)BitConverter.ToUInt32(packetBuffer, 0), Socket.RemoteEndPoint);
            base.Send(packetBuffer);
        }

        public void Send(IFFPacket packet) => Send(packet.Buffer);
    }
}
