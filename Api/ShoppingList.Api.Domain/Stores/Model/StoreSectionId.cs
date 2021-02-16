using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model
{
    public class StoreSectionId : GenericPrimitive<int>
    {
        public StoreSectionId(int value) : base(value)
        {
        }
    }
}