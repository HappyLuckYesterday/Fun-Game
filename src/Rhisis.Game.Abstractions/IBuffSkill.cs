namespace Rhisis.Game.Abstractions
{
    public interface IBuffSkill : IBuff
    {
        /// <summary>
        /// Gets the buff skill database id.
        /// </summary>
        int? DatabaseId { get; }

        /// <summary>
        /// Gets the buff skill id.
        /// </summary>
        int SkillId { get; }

        /// <summary>
        /// Gets the buff skill name.
        /// </summary>
        string SkillName { get; }

        /// <summary>
        /// Gets the buff skill level.
        /// </summary>
        int SkillLevel { get; }
    }
}
