using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Ports
{
    public class ItemRepositoryMock
    {
        private readonly Mock<IItemRepository> mock;

        public ItemRepositoryMock(Mock<IItemRepository> mock)
        {
            this.mock = mock;
        }

        public ItemRepositoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IItemRepository>>();
        }

        public void SetupFindByAsync(ItemId itemId, IStoreItem returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemId>(id => id == itemId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public void SetupFindByAsync(IEnumerable<ItemId> itemIds, IEnumerable<IStoreItem> returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<IEnumerable<ItemId>>(ids => ids.SequenceEqual(itemIds)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public void SetupFindByAsync(TemporaryItemId temporaryItemId, IStoreItem returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<TemporaryItemId>(id => id == temporaryItemId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public void SetupFindActiveByAsync(ItemCategoryId itemCategoryId, IEnumerable<IStoreItem> returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<ItemCategoryId>(id => id == itemCategoryId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public void VerifyFindByAsync(ItemId storeItemId)
        {
            mock.Verify(
                i => i.FindByAsync(
                        It.Is<ItemId>(id => id == storeItemId),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        public void VerifyFindByAsync(TemporaryItemId temporaryItemId)
        {
            mock.Verify(
                i => i.FindByAsync(
                        It.Is<TemporaryItemId>(id => id == temporaryItemId),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        public void VerifyStoreAsyncOnce(IStoreItem storeItem)
        {
            mock.Verify(
                i => i.StoreAsync(
                    It.Is<IStoreItem>(item => item == storeItem),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public void VerifyStoreAsyncNever()
        {
            mock.Verify(
                i => i.StoreAsync(
                    It.IsAny<IStoreItem>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}