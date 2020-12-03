using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.ShoppingList
{
    public class FinishShoppingListTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var cf = new CommonFixture();
            var shoppingListItemFixture = new ShoppingListItemFixture(cf);

            yield return new object[]
            {
                    new List<ShoppingListItem>
                    {
                        shoppingListItemFixture.GetShoppingListItem(isInBasket: false),
                        shoppingListItemFixture.GetShoppingListItem(isInBasket: false)
                    },
                    new List<ShoppingListItem>
                    {
                        shoppingListItemFixture.GetShoppingListItem(isInBasket: true),
                        shoppingListItemFixture.GetShoppingListItem(isInBasket: true)
                    }
            };
            yield return new object[]
            {
                    new List<ShoppingListItem>
                    {
                    },
                    new List<ShoppingListItem>
                    {
                        shoppingListItemFixture.GetShoppingListItem(isInBasket: true),
                        shoppingListItemFixture.GetShoppingListItem(isInBasket: true)
                    }
            };
            yield return new object[]
            {
                    new List<ShoppingListItem>
                    {
                        shoppingListItemFixture.GetShoppingListItem(isInBasket: false),
                        shoppingListItemFixture.GetShoppingListItem(isInBasket: false)
                    },
                    new List<ShoppingListItem>
                    {
                    }
            };
            yield return new object[]
            {
                    new List<ShoppingListItem>
                    {
                    },
                    new List<ShoppingListItem>
                    {
                    }
            };
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}