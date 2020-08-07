using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.Trade;

namespace Rhisis.World.Systems.Initializers
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class PlayerTradeInitializer : IGameSystemLifeCycle
    {
        public int Order => 2;

        public void Initialize(IPlayerEntity player)
        {
            player.Trade = new TradeComponent(TradeSystem.MaxTrade);
        }

        public void Save(IPlayerEntity player)
        {
            // Nothing to do.
        }
    }
}
