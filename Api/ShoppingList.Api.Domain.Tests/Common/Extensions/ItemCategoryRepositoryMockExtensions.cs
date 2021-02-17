using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class ItemCategoryRepositoryMockExtensions
    {
        public static void SetupFindByAsync(this Mock<IItemCategoryRepository> mock, ItemCategoryId itemCategoryId,
            IItemCategory returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == itemCategoryId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public static void VerifyStoreAsyncOnce(this Mock<IItemCategoryRepository> mock, IItemCategory itemCategory)
        {
            mock.
                Verify(i => i.StoreAsync(
                    It.Is<IItemCategory>(cat => cat == itemCategory),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}