using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model
{
    public class StoreSectionId : GenericPrimitive<int>
    {
        public StoreSectionId(int value) : base(value)
        {
        }

        public StoreItemSectionId AsStoreItemSectionId()
        {
            return new StoreItemSectionId(Value);
        }
    }
}