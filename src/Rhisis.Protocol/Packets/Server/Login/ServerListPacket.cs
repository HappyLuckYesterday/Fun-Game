using Rhisis.Abstractions.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using ClusterInfo = Rhisis.Abstractions.Server.Cluster;

namespace Rhisis.Protocol.Packets.Server.Login;

public class ServerListPacket : FFPacket
{
    public ServerListPacket(string username, IEnumerable<ClusterInfo> clusters)
        : base(PacketType.SRVR_LIST)
    {
        WriteInt32(0);
        WriteByte(1);
        WriteString(username);
        WriteInt32(clusters.Sum(x => x.Channels.Count) + clusters.Count());

        foreach (ClusterInfo cluster in clusters)
        {
            WriteInt32(-1);
            WriteInt32(cluster.Id);
            WriteString(cluster.Name);
            WriteString(cluster.Host);
            WriteInt32(0);
            WriteInt32(0);
            WriteInt32(Convert.ToInt32(cluster.IsEnabled));
            WriteInt32(0);

            foreach (WorldChannel world in cluster.Channels)
            {
                WriteInt32(cluster.Id);
                WriteInt32(world.Id);
                WriteString(world.Name);
                WriteString(world.Host);
                WriteInt32(0);
                WriteInt32(world.ConnectedUsers);
                WriteInt32(Convert.ToInt32(world.IsEnabled));
                WriteInt32(world.MaximumUsers);
            }
        }
    }
}
