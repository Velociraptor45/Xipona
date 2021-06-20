using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.ItemCategories.Ports
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

        public void SetupFindByAsync(IEnumerable<ItemCategoryId> itemCategoryIds, IEnumerable<IItemCategory> returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<IEnumerable<ItemCategoryId>>(ids => ids.SequenceEqual(itemCategoryIds)),
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