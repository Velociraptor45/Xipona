using AutoFixture;
using ShoppingList.Api.Domain.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models
{
    public class ShoppingListItemFixture
    {
        private readonly CommonFixture commonFixture;

        public ShoppingListItemFixture()
        {
            commonFixture = new CommonFixture();
        }

        public ShoppingListItem GetShoppingListItemWithId(ShoppingListItemId id)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            return fixture.Create<ShoppingListItem>();
        }

        public StoreItem GetStoreItemWithId(StoreItemId id)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            return fixture.Create<StoreItem>();
        }
    }
}