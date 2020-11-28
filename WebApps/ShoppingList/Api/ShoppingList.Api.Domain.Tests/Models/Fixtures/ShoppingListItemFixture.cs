using AutoFixture;
using ShoppingList.Api.Core.Tests.AutoFixture;
using ShoppingList.Api.Domain.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures
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

        public ShoppingListItem GetShoppingListItemWithId(int id)
        {
            return GetShoppingListItemWithId(new ShoppingListItemId(id));
        }

        public ShoppingListItem GetShoppingListItemWithId(Guid id)
        {
            return GetShoppingListItemWithId(new ShoppingListItemId(id));
        }

        public ShoppingListItem GetShoppingListItem()
        {
            return GetShoppingListItemWithId(commonFixture.NextInt());
        }

        public ShoppingListItem GetShoppingListItem(bool isInBasket)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isInBasket", isInBasket);
            return fixture.Create<ShoppingListItem>();
        }
    }
}