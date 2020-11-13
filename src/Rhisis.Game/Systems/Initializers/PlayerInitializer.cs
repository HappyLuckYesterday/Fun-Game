using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using System;
using System.Linq;

namespace Rhisis.Game.Systems.Initializers
{
    [Injectable]
    public class PlayerInitializer : IPlayerInitializer
    {
        private readonly IRhisisDatabase _database;

        public PlayerInitializer(IRhisisDatabase database)
        {
            _database = database;
        }

        public void Load(IPlayer player)
        {
            // Nothing to load.
        }

        public void Save(IPlayer player)
        {
            DbCharacter character = _database.Characters.FirstOrDefault(x => x.Id == player.CharacterId);

            if (character != null)
            {
                character.LastConnectionTime = player.LoggedInAt;
                character.PlayTime += (long)(DateTime.UtcNow - player.LoggedInAt).TotalSeconds;

                character.PosX = player.Position.X;
                character.PosY = player.Position.Y;
                character.PosZ = player.Position.Z;
                character.Angle = player.Angle;
                character.MapId = player.Map.Id;
                character.MapLayerId = player.MapLayer.Id;
                character.Gender = (byte)player.Appearence.Gender;
                character.HairColor = player.Appearence.HairColor;
                character.HairId = player.Appearence.HairId;
                character.FaceId = player.Appearence.FaceId;
                character.SkinSetId = player.Appearence.SkinSetId;
                character.Level = player.Level;

                character.JobId = (int)player.Job.Id;
                character.Gold = player.Gold.Amount;
                character.Experience = player.Experience.Amount;

                character.Strength = player.Statistics.Strength;
                character.Stamina = player.Statistics.Stamina;
                character.Dexterity = player.Statistics.Dexterity;
                character.Intelligence = player.Statistics.Intelligence;
                character.StatPoints = player.Statistics.AvailablePoints;
                character.SkillPoints = player.SkillTree.SkillPoints;

                character.Hp = player.Health.Hp;
                character.Mp = player.Health.Mp;
                character.Fp = player.Health.Fp;

                _database.SaveChanges();
            }
        }
    }
}
