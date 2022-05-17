namespace Rhisis.Protocol.Packets.Server.World.Friends
{
    public class FriendInterceptPacket : FFPacket
    {
        public FriendInterceptPacket(int playerId, int friendId)
            : base(PacketType.FRIENDINTERCEPTSTATE)
        {
            WriteUInt32((uint)playerId);
            WriteUInt32((uint)friendId);
        }
    }
}
