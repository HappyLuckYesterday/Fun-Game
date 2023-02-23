namespace Rhisis.Protocol.Packets.Cluster.Client;

public class PreJoinPacket
{
    /// <summary>
    /// Gets the account username.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the character id that is joining the game.
    /// </summary>
    public int CharacterId { get; }

    /// <summary>
    /// Gets the character name that is joining the game.
    /// </summary>
    public string CharacterName { get; }

    /// <summary>
    /// Gets the character's bank code.
    /// </summary>
    public int BankCode { get; }

    public PreJoinPacket(FFPacket packet)
    {
        Username = packet.ReadString();
        CharacterId = packet.ReadInt32();
        CharacterName = packet.ReadString();
        BankCode = packet.ReadInt32();
    }
}
