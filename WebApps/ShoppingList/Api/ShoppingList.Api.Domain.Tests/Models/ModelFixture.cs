using AutoFixture;
using ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models
{
    public static class ModelFixture
    {
        public static ShoppingListItem GetShoppingListItemWithId(ShoppingListItemId id)
        {
            var fixture = CommonFixture.GetNewFixture();
            fixture.Inject(id);
            return fixture.Create<ShoppingListItem>();
        }

        public static StoreItem GetStoreItemWithId(StoreItemId id)
        {
            var fixture = CommonFixture.GetNewFixture();
            fixture.Inject(id);
            return fixture.Create<StoreItem>();
        }
    }
}