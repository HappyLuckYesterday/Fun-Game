using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IPlayer : IHuman
    {
        IGameConnection Connection { get; }

        int CharacterId { get; }

        long Experience { get; set; }

        int Gold { get; set; }

        int Slot { get; }

        AuthorityType Authority { get; }

        ModeType Mode { get; set; }

        JobData Job { get; set; }

        IPlayerStatistics Statistics { get; }

        IInventory Inventory { get; }

        void Speak(string text);

        void Shout(string text);
    }
}
