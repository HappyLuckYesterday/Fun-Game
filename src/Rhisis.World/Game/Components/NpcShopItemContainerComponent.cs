using Rhisis.World.Game.Structures;

namespace Rhisis.World.Game.Components
{
    public class NpcShopItemContainerComponent : ItemContainerComponent<Item>
    {
        public NpcShopItemContainerComponent(int maxCapacity) 
            : base(maxCapacity)
        {
        }
    }
}
