using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Protocol.Packets.Login.Server;

public class ServerListPacket : FFPacket
{
    public ServerListPacket(string username, IEnumerable<ClusterInfo> clusters)
        : base(PacketType.SRVR_LIST)
    {
        WriteInt32(0); // Authentication key
        WriteByte(1);
        WriteString(username);
        WriteInt32(clusters.Sum(x => x.Channels.Count) + clusters.Count());

        foreach (ClusterInfo cluster in clusters)
        {
            WriteInt32(-1); // Parent server id
            WriteInt32(cluster.Id);
            WriteString(cluster.Name);
            WriteString(cluster.Ip);
            WriteInt32(0); // b18 ?
            WriteInt32(0); // Connected count
            WriteInt32(Convert.ToInt32(cluster.IsEnabled));
            WriteInt32(0); // Maximum users

            foreach (WorldChannelInfo world in cluster.Channels)
            {
                WriteInt32(cluster.Id);
                WriteInt32(world.Id);
                WriteString(world.Name);
                WriteString(world.Ip);
                WriteInt32(0); // b18 ?
                WriteInt32(world.ConnectedUsers);
                WriteInt32(Convert.ToInt32(world.IsEnabled));
                WriteInt32(world.MaximumUsers);
            }
        }
    }
}