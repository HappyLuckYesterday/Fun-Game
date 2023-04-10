using Rhisis.Game;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Friends;

/// <summary>
/// Provides a data structure representing the add friend packet.
/// </summary>
public class AddFriendPacket
{
    /// <summary>
    /// Gets the sender player id.
    /// </summary>
    public uint SenderId { get; private set; }

    /// <summary>
    /// Gets the friend player id.
    /// </summary>
    public uint FriendId { get; private set; }

    /// <summary>
    /// Gets the sender gender.
    /// </summary>
    public GenderType SenderGender { get; private set; }

    /// <summary>
    /// Gets the friend gender.
    /// </summary>
    public GenderType FriendGender { get; private set; }

    /// <summary>
    /// Gets the sender job.
    /// </summary>
    public DefineJob.Job SenderJob { get; private set; }

    /// <summary>
    /// Gets the friend job.
    /// </summary>
    public DefineJob.Job FriendJob { get; private set; }

    public AddFriendPacket(FFPacket packet)
    {
        SenderId = packet.ReadUInt32();
        FriendId = packet.ReadUInt32();
        SenderGender = (GenderType)packet.ReadByte();
        FriendGender = (GenderType)packet.ReadByte();
        SenderJob = (DefineJob.Job)packet.ReadInt32();
        FriendJob = (DefineJob.Job)packet.ReadInt32();
    }
}
