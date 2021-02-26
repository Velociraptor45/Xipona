using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemSectionId : GenericPrimitive<int>
    {
        public StoreItemSectionId(int value) : base(value)
        {
        }
    }
}