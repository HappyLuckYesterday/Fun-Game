using Rhisis.Game;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Packets.Cluster.Server;

public class PlayerListPacket : FFPacket
{
    public PlayerListPacket(int authenticationKey, IEnumerable<SelectableCharacter> characters)
        : base(PacketType.PLAYER_LIST)
    {
        WriteInt32(authenticationKey);
        WriteInt32(characters.Count());

        foreach (SelectableCharacter character in characters)
        {
            WriteInt32(character.Slot);
            WriteInt32(1); // this number represents the selected character in the window
            WriteInt32(character.MapId);
            WriteInt32(0x0B + (byte)character.Gender); // Model id
            WriteString(character.Name);
            WriteSingle(character.PositionX);
            WriteSingle(character.PositionY);
            WriteSingle(character.PositionZ);
            WriteInt32(character.Id);
            WriteInt32(0); // Party id
            WriteInt32(0); // Guild id
            WriteInt32(0); // War Id
            WriteInt32(character.SkinSetId);
            WriteInt32(character.HairId);
            WriteUInt32(character.HairColor);
            WriteInt32(character.FaceId);
            WriteByte((byte)character.Gender);
            WriteInt32(character.JobId);
            WriteInt32(character.Level);
            WriteInt32(0); // Job Level (Maybe master or hero ?)
            WriteInt32(character.Strength);
            WriteInt32(character.Stamina);
            WriteInt32(character.Dexterity);
            WriteInt32(character.Intelligence);
            WriteInt32(0); // Mode ??

            WriteInt32(character.EquipedItems.Count());

            foreach (int equipedItemId in character.EquipedItems)
            {
                WriteInt32(equipedItemId);
            }
        }

        WriteInt32(0); // Messenger?
    }
}
