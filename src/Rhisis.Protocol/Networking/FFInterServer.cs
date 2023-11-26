using LiteNetwork.Server;
using System;

namespace Rhisis.Protocol.Networking;

public class FFInterServer<TUser> : LiteServer<TUser> where TUser : FFInterServerConnection
{
    public FFInterServer(LiteServerOptions options, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
    }
}
