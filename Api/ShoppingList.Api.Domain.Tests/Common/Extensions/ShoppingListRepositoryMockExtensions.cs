using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class ShoppingListRepositoryMockExtensions
    {
        public static void SetupFindActiveByAsync(this Mock<IShoppingListRepository> mock, StoreItemId storeItemId,
            IEnumerable<IShoppingList> returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<StoreItemId>(id => id == storeItemId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }
    }
}