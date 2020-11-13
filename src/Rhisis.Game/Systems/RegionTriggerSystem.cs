using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Systems;
using System.Linq;

namespace Rhisis.World.Systems
{
    [Injectable]
    public sealed class RegionTriggerSystem : IRegionTriggerSystem
    {
        /// <inheritdoc />
        public void CheckWrapzones(IPlayer player)
        {
            IMapTriggerRegion triggerRegion = player.Map.Regions
                .OfType<IMapTriggerRegion>()
                .FirstOrDefault(x => x.IsWrapzone && x.GetRectangle().Contains(player.Position));

            if (triggerRegion != null && triggerRegion.IsWrapzone)
            {
                player.Teleport(triggerRegion.DestinationMapPosition, triggerRegion.DestinationMapId);
            }
        }
    }
}
