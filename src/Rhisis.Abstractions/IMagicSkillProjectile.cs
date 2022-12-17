namespace Rhisis.Abstractions;

public interface IMagicSkillProjectile : IProjectile
{
    /// <summary>
    /// Gets the projectile skill.
    /// </summary>
    ISkill Skill { get; }
}
