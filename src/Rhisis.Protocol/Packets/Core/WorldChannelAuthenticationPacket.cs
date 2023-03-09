namespace Rhisis.Protocol.Packets.Core;

public sealed record WorldChannelAuthenticationPacket(string ClusterName, string Name, string Ip, int Port, string MasterPassword, int MaximumUsers);