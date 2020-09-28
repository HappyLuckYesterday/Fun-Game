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

        IExperience Experience { get; }

        IGold Gold { get; }

        int Slot { get; }

        int DeathLevel { get; set; }

        AuthorityType Authority { get; }

        ModeType Mode { get; set; }

        JobData Job { get; set; }

        IPlayerStatistics Statistics { get; }

        IInventory Inventory { get; }

        string CurrentNpcShopName { get; set; }

        void Speak(string text);

        void Shout(string text);
    }
}
