using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhisis.Database.Entities
{
    public class DbSkillBuff
    {
        public int Id { get; set; }

        public int CharacterId { get; set; }

        public DbCharacter Character { get; set; }

        public int SkillId { get; set; }

        public int SkillLevel { get; set; }

        public int RemainingTime { get; set; }

        [NotMapped]
        public bool IsExpired => RemainingTime <= 0;

        public ICollection<DbSkillBuffAttribute> Attributes { get; set; }
    }
}
