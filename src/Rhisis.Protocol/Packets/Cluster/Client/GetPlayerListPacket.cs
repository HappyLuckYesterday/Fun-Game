namespace Rhisis.Protocol.Packets.Cluster.Client;

public sealed class GetPlayerListPacket
{
    /// <summary>
    /// Gets the client build version.
    /// </summary>
    public string BuildVersion { get; }

    /// <summary>
    /// Gets the client authentication key.
    /// </summary>
    public int AuthenticationKey { get; }

    /// <summary>
    /// Gets the account username.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the account's password.
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Gets the client's server id.
    /// </summary>
    public int ServerId { get; }

    public GetPlayerListPacket(FFPacket packet)
    {
        BuildVersion = packet.ReadString();
        AuthenticationKey = packet.ReadInt32();
        Username = packet.ReadString();
        Password = packet.ReadString();
        ServerId = packet.ReadInt32();
    }
}
