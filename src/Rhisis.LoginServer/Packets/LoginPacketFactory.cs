using Rhisis.LoginServer.Client;
using Rhisis.Network;
using Rhisis.Network.Core;
using Rhisis.Network.Core.Servers;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer.Packets
{
    public class LoginPacketFactory : ILoginPacketFactory
    {
        /// <inheritdoc />
        public void SendWelcome(ILoginClient client, uint sessionId)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(sessionId);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendPong(ILoginClient client, int time)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PING);
                packet.Write(time);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendLoginError(ILoginClient client, ErrorType error)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.ERROR);
                packet.Write((int)error);

                client.Send(packet);
            }
        }

        /// <inheritdoc />
        public void SendServerList(ILoginClient client, string username, IEnumerable<Cluster> clusters)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.SRVR_LIST);
                packet.Write(0);
                packet.Write<byte>(1);
                packet.Write(username);
                packet.Write(clusters.Sum(x => x.Channels.Count) + clusters.Count());

                foreach (Cluster cluster in clusters)
                {
                    packet.Write(-1);
                    packet.Write(cluster.Id);
                    packet.Write(cluster.Name);
                    packet.Write(cluster.Host);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write(1);
                    packet.Write(0);

                    foreach (WorldChannel world in cluster.Channels)
                    {
                        packet.Write(cluster.Id);
                        packet.Write(world.Id);
                        packet.Write(world.Name);
                        packet.Write(world.Host);
                        packet.Write(0);
                        packet.Write(0);
                        packet.Write(1);
                        packet.Write(100); // Capacity
                    }
                }

                client.Send(packet);
            }
        }
    }
}
