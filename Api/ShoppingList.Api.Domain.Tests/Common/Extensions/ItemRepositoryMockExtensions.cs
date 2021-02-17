using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System.Collections.Generic;
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

        public static void SetupFindActiveByAsync(this Mock<IItemRepository> mock, ItemCategoryId itemCategoryId,
            IEnumerable<IStoreItem> returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<ItemCategoryId>(id => id == itemCategoryId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public static void VerifyStoreAsyncOnce(this Mock<IItemRepository> mock, IStoreItem storeItem)
        {
            mock.Verify(
                i => i.StoreAsync(
                    It.Is<IStoreItem>(item => item == storeItem),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public static void VerifyStoreAsyncNever(this Mock<IItemRepository> mock)
        {
            mock.Verify(
                i => i.StoreAsync(
                    It.IsAny<IStoreItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}