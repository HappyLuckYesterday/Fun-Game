namespace Rhisis.World.Game.Structures
{
    public struct SkillExtraBonus
    {
        public int CastingTime { get; }

        public int Heal { get; }

        public int Damages { get; }

        public SkillExtraBonus(int castingTime, int healPoints, int damagePoints)
        {
            CastingTime = castingTime;
            Heal = healPoints;
            Damages = damagePoints;
        }
    }
}