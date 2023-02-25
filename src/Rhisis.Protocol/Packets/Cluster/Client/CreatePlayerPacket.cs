using Rhisis.Game;

namespace Rhisis.Protocol.Packets.Cluster.Client;

public class CreatePlayerPacket
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
    /// Gets the character slot.
    /// </summary>
    public int Slot { get; }

    /// <summary>
    /// Gets the character name.
    /// </summary>
    public string CharacterName { get; }

    /// <summary>
    /// Gets the character face id.
    /// </summary>
    public int FaceId { get; }

    /// <summary>
    /// Gets the character costume id.
    /// </summary>
    /// <remarks>
    /// This field is not used in FLYFF.
    /// </remarks>
    public int CostumeId { get; }

    /// <summary>
    /// Gets the character skin set id.
    /// </summary>
    /// <remarks>
    /// This field is not used in FLYFF.
    /// </remarks>
    public int SkinSet { get; }

    /// <summary>
    /// Gets the character hair model (3D mesh) id.
    /// </summary>
    public int HairMeshId { get; }

    /// <summary>
    /// Gets the character haid color.
    /// </summary>
    public uint HairColor { get; }

    /// <summary>
    /// Gets the character gender.
    /// </summary>
    public byte Gender { get; }

    /// <summary>
    /// Gets the character begin job.
    /// </summary>
    public DefineJob.Job Job { get; }

    /// <summary>
    /// Gets the character head model (3D mesh) id.
    /// </summary>
    public int HeadMesh { get; }

    /// <summary>
    /// Gets the character bank password.
    /// </summary>
    public int BankPassword { get; }

    /// <summary>
    /// Gets the character authentication key.
    /// </summary>
    public int AuthenticationKey { get; }

    public CreatePlayerPacket(FFPacket packet)
    {
        Username = packet.ReadString();
        Password = packet.ReadString();
        Slot = packet.ReadByte();
        CharacterName = packet.ReadString();
        FaceId = packet.ReadByte();
        CostumeId = packet.ReadByte();
        SkinSet = packet.ReadByte();
        HairMeshId = packet.ReadByte();
        HairColor = packet.ReadUInt32();
        Gender = packet.ReadByte();
        Job = (DefineJob.Job)packet.ReadByte();
        HeadMesh = packet.ReadByte();
        BankPassword = packet.ReadInt32();
        AuthenticationKey = packet.ReadInt32();
    }
}
