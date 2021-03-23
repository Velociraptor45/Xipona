using ProjectHermes.ShoppingList.Api.Core;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public class SectionId : GenericPrimitive<int>
    {
        public SectionId(int value) : base(value)
        {
        }

        public StoreItemSectionId AsStoreItemSectionId()
        {
            return new StoreItemSectionId(Value);
        }
    }
}