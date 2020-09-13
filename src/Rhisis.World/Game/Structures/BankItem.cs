using Rhisis.Game.Common.Resources;

namespace Rhisis.World.Game.Structures
{
    public class BankItem : Item
    {
        public BankItem(ItemData itemData, int quantity, int index, int slot, int? databaseId) 
            : base(itemData, quantity, databaseId)
        {
            Index = index;
            Slot = slot;
        }
    }
}