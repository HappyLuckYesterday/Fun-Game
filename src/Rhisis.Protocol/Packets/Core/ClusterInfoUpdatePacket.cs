using System.Collections.Generic;

namespace Rhisis.Protocol.Packets.Core;

public sealed record ClusterInfoUpdatePacket(List<WorldChannelInfo> Channels);
