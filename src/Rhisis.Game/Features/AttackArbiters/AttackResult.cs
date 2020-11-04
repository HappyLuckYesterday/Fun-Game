using Rhisis.Game.Common;

namespace Rhisis.Game.Features.AttackArbiters
{
    public class AttackResult
    {
        public int Damages { get; set; }

        public AttackFlags Flags { get; set; }

        public static AttackResult Miss()
        {
            return new AttackResult
            {
                Flags = AttackFlags.AF_MISS
            };
        }

        public static AttackResult Success(int damages, AttackFlags attackFlags)
        {
            return new AttackResult
            {
                Damages = damages,
                Flags = attackFlags
            };
        }
    }
}
