using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Mocks
{
    public class ItemCategoryRepositoryMock
    {
        private readonly Mock<IItemCategoryRepository> mock;

        public ItemCategoryRepositoryMock(Mock<IItemCategoryRepository> mock)
        {
            this.mock = mock;
        }

        public ItemCategoryRepositoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IItemCategoryRepository>>();
        }

        public void SetupFindByAsync(ItemCategoryId itemCategoryId, IItemCategory returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == itemCategoryId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public void VerifyStoreAsyncOnce(IItemCategory itemCategory)
        {
            mock.
                Verify(i => i.StoreAsync(
                    It.Is<IItemCategory>(cat => cat == itemCategory),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}