using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class ItemRepositoryMockExtensions
    {
        public static void SetupFindByAsync(this Mock<IItemRepository> mock, StoreItemId storeItemId,
            IStoreItem returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreItemId>(id => id == storeItemId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }
    }
}