namespace Rhisis.Protocol.Packets.Cluster.Client;

public class DeletePlayerPacket
{
    /// <summary>
    /// Gets the account username.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the account password.
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Gets the account password verfication.
    /// </summary>
    public string PasswordConfirmation { get; }

    /// <summary>
    /// Gets the character id to be deleted.
    /// </summary>
    public int CharacterId { get; }

    /// <summary>
    /// Gets the user authentication key.
    /// </summary>
    public int AuthenticationKey { get; }

    public DeletePlayerPacket(FFPacket packet)
    {
        Username = packet.ReadString();
        Password = packet.ReadString();
        PasswordConfirmation = packet.ReadString();
        CharacterId = packet.ReadInt32();
        AuthenticationKey = packet.ReadInt32();
    }
}
