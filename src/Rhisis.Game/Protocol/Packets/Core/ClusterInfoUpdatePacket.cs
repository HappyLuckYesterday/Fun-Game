using Rhisis.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Protocol.Packets.Core;

public sealed record ClusterInfoUpdatePacket(List<WorldChannelInfo> Channels);
