using AutoFixture;
using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}