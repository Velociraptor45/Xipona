using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.DeleteItem
{
    public class HandleAsyncWithShoppingListsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var commonFixture = new CommonFixture();
            var shoppingListMockFixture = new ShoppingListMockFixture();

            yield return new object[]
            {
                shoppingListMockFixture.Create().ToMonoList()
            };
            yield return new object[]
            {
                shoppingListMockFixture.CreateMany(3).ToList()
            };
            yield return new object[]
            {
                new List<ShoppingListMock>()
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}