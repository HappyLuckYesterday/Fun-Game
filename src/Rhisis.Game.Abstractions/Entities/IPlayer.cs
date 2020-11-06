using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Features.Chat;
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

        new IPlayerStatistics Statistics { get; }

        IInventory Inventory { get; }

        IChat Chat { get; }

        IBattle Battle { get; }

        IQuestDiary Quests { get; }

        string CurrentNpcShopName { get; set; }

        void Teleport(Vector3 position, bool sendToPlayer = true);

        void Teleport(Vector3 position, int mapId, bool sendToPlayer = true);
    }
}
