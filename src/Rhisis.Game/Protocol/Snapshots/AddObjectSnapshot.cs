using Rhisis.Game.Abstractions.Entities;
using System;

namespace Rhisis.Network.Snapshots
{
    public class AddObjectSnapshot : FFSnapshot
    {
        public AddObjectSnapshot(IWorldObject worldObject)
            : base(SnapshotType.ADD_OBJ, (int)worldObject.Id)
        {
            Write((byte)worldObject.Type);
            Write(worldObject.ModelId);
            Write((byte)worldObject.Type);
            Write(worldObject.ModelId);
            Write(worldObject.Size);
            Write(worldObject.Position.X);
            Write(worldObject.Position.Y);
            Write(worldObject.Position.Z);
            Write((short)(worldObject.Angle * 10));
            Write(worldObject.Id);

            switch (worldObject)
            {
                case IPlayer player:
                    {
                        Write<short>(0); // m_dwMotion
                        Write<byte>(1); // m_bPlayer
                        Write(player.Health.Hp); // HP
                        Write((int)player.ObjectState); // moving flags
                        Write((int)player.ObjectStateFlags); // motion flags
                        Write<byte>(1); // m_dwBelligerence

                        Write(-1); // m_dwMoverSfxId

                        Write(player.Name);
                        Write(player.Appearence.Gender);
                        Write((byte)player.Appearence.SkinSetId);
                        Write((byte)player.Appearence.HairId);
                        Write(player.Appearence.HairColor);
                        Write((byte)player.Appearence.FaceId);
                        Write(player.CharacterId);
                        Write((byte)player.Job.Id);

                        Write((short)player.Statistics.Strength);
                        Write((short)player.Statistics.Stamina);
                        Write((short)player.Statistics.Dexterity);
                        Write((short)player.Statistics.Intelligence);

                        Write((short)player.Level); // Level
                        Write(-1); // Fuel
                        Write(0); // Actuel fuel

                        // Guilds

                        Write<byte>(0); // have guild or not
                        Write(0); // guild cloak

                        // Party

                        Write<byte>(0); // have party or not

                        Write((byte)player.Authority); // authority
                        Write((uint)player.Mode); // mode
                        Write(0); // state mode
                        Write(0); // item used ??
                        Write(0); // last pk time.
                        Write(0); // karma
                        Write(0); // pk propensity
                        Write(0); // pk exp
                        Write(0); // fame
                        Write<byte>(0); // duel
                        Write(-1); // titles
                    }
                    break;
                case IMonster monster:
                    {
                        throw new NotImplementedException(monster.Name);
                    }
                    break;
                case INpc npc:
                    {
                        throw new NotImplementedException(npc.Name);
                    }
                    break;
            }
        }
    }
}
