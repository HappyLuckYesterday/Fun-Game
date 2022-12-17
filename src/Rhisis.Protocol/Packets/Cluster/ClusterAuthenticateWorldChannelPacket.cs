using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Protocol.Core;

namespace Rhisis.Protocol.Packets.Cluster;

public class ClusterAuthenticateWorldChannelPacket : CorePacket
{
    public ClusterAuthenticateWorldChannelPacket(WorldOptions options, int connectedPlayers)
        : base(CorePacketType.AuthenticationRequest)
    {
        WriteString(options.ClusterCache.Password);
        WriteByte((byte)options.Id);
        WriteString(options.Name);
        WriteString(options.Host);
        WriteUInt16((ushort)options.Port);
        WriteInt32(connectedPlayers);
        WriteInt32(options.MaximumUsers);
    }
}
