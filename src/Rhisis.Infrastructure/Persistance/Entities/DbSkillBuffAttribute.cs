namespace Rhisis.Infrastructure.Persistance.Entities
{
    public class DbSkillBuffAttribute
    {
        public int SkillBuffId { get; set; }

        public DbSkillBuff SkillBuff { get; set; }

        public int AttributeId { get; set; }

        public DbAttribute Attribute { get; set; }

        public int Value { get; set; }
    }
}
