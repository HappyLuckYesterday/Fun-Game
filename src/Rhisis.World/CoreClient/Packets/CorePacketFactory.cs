﻿using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Network.Core;
using Sylver.Network.Data;

namespace Rhisis.World.CoreClient.Packets
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class CorePacketFactory : ICorePacketFactory
    {
        /// <inheritdoc />
        public void SendAuthentication(ICoreClient client, WorldConfiguration worldConfiguration)
        {
            using (var packet = new NetPacket())
            {
                packet.Write((uint)CorePacketType.Authenticate);
                packet.Write(worldConfiguration.Id);
                packet.Write(worldConfiguration.Name);
                packet.Write(worldConfiguration.Host);
                packet.Write(worldConfiguration.Port);
                packet.Write((byte)ServerType.World);
                packet.Write(worldConfiguration.ClusterId);

                // TODO: add more information to packet if needed.
                client.Send(packet);
            }
        }
    }
}
