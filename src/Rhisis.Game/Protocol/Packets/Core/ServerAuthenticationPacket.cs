namespace Rhisis.Game.Protocol.Packets.Core;

public sealed record ServerAuthenticationPacket(string Name, string Ip, int Port, string MasterPassword);
