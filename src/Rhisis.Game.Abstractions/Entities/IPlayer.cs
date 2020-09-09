using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Structures.Game;
using Rhisis.Game.Abstractions.Components;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IPlayer : IHuman
    {
        int CharacterId { get; }

        long Experience { get; set; }

        int Gold { get; set; }

        int Slot { get; }

        AuthorityType Authority { get; }

        ModeType Mode { get; set; }

        JobData Job { get; set; }

        IPlayerStatistics Statistics { get; }
    }
}
