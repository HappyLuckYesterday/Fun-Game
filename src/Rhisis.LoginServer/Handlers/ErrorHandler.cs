using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.LoginServer.Handlers
{
    [Handler]
    public class ErrorHandler
    {
        /// <summary>
        /// Disconnects the client when it receives an error packet.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="_"></param>
        [HandlerAction(PacketType.ERROR)]
        public void Execute(ILoginUser client, CorePacket _)
        {
            client.Disconnect();
        }
    }
}