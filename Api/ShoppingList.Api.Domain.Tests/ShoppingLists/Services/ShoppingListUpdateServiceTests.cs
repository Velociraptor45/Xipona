using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services
{
    public class ShoppingListUpdateServiceTests
    {
        private readonly LocalFixture _local;

        public ShoppingListUpdateServiceTests()
        {
            _local = new LocalFixture();
        }

        #region ExchangeItemAsync

        [Fact]
        public async Task ExchangeItemAsync_WithOldItemIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var service = _local.CreateService();
            _local.SetupNewItem();
            _local.SetupOldItemNull();

            // Act
            Func<Task> function = async () => await service.ExchangeItemAsync(_local.OldItem?.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var service = _local.CreateService();
            _local.SetupNewItemNull();
            _local.SetupOldItem();

            // Act
            Func<Task> function = async () => await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithOldItemOnNoShoppingLists_ShouldDoNothing()
        {
            // Arrange
            var service = _local.CreateService();

            _local.SetupOldItem();
            _local.SetupNewItem();
            _local.SetupFindingNoShoppingList();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoreShoppingListNever();
            }
        }

        #region WithNewItemNotAvailableForShoppingList

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldRemoveOldItem()
        {
            // Arrange
            _local.SetupWithNewItemNotAvailableForShoppingList();

            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyRemoveItemOnce();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldNotAddItemToShoppingList()
        {
            // Arrange
            _local.SetupWithNewItemNotAvailableForShoppingList();

            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyAddItemToShoppingListNever();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldStoreShoppingList()
        {
            // Arrange
            _local.SetupWithNewItemNotAvailableForShoppingList();

            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoreShoppingListOnce();
            }
        }

        #endregion WithNewItemNotAvailableForShoppingList

        #region WithNewItemAvailableForShoppingListAndInBasket

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldRemoveOldItemFromShoppingList()
        {
            // Arrange
            _local.SetupWithNewItemAvailableForShoppingListAndInBasket();
            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyRemoveItemOnce();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldStoreShoppingList()
        {
            // Arrange
            _local.SetupWithNewItemAvailableForShoppingListAndInBasket();
            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoreShoppingListOnce();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldAddItemToShoppingList()
        {
            // Arrange
            _local.SetupWithNewItemAvailableForShoppingListAndInBasket();
            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyAddItemToShoppingListOnce();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldPutItemInBasket()
        {
            // Arrange
            _local.SetupWithNewItemAvailableForShoppingListAndInBasket();
            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyPutItemInBasketOnce();
            }
        }

        #endregion WithNewItemAvailableForShoppingListAndInBasket

        #region WithNewItemAvailableForShoppingListAndNotInBasket

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldRemoveOldItemFromShoppingList()
        {
            // Arrange
            _local.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyRemoveItemOnce();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldStoreShoppingList()
        {
            // Arrange
            _local.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoreShoppingListOnce();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldAddNewItemToShoppingList()
        {
            // Arrange
            _local.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyAddItemToShoppingListOnce();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldNotPutItemInBasket()
        {
            // Arrange
            _local.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
            var service = _local.CreateService();

            // Act
            await service.ExchangeItemAsync(_local.OldItem.Id, _local.NewItem, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyPutItemInBasketNever();
            }
        }

        #endregion WithNewItemAvailableForShoppingListAndNotInBasket

        #endregion ExchangeItemAsync

        private class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public AddItemToShoppingListServiceMock AddItemToShoppingListServiceMock { get; }

            public ShoppingListMock ShoppingListMock { get; private set; }
            public IStoreItem NewItem { get; private set; }
            public IShoppingListItem OldItem { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(Fixture);
                AddItemToShoppingListServiceMock = new AddItemToShoppingListServiceMock(Fixture);
            }

            public ShoppingListUpdateService CreateService()
            {
                return Fixture.Create<ShoppingListUpdateService>();
            }

            public void SetupNewItemMatchingShoppingList()
            {
                SetupNewItemForStore(ShoppingListMock.Object.StoreId);
            }

            public void SetupNewItemNotMatchingShoppingList()
            {
                var storeId = new StoreId(CommonFixture.NextInt(ShoppingListMock.Object.StoreId.Value));
                SetupNewItemForStore(storeId);
            }

            private void SetupNewItemForStore(StoreId storeId)
            {
                var availability = StoreItemAvailabilityMother.Initial()
                    .WithStoreId(storeId)
                    .Create();
                NewItem = StoreItemMother.Initial().WithAvailability(availability).Create();
            }

            public void SetupNewItem()
            {
                NewItem = StoreItemMother.Initial().Create();
            }

            public void SetupNewItemNull()
            {
                NewItem = null;
            }

            public void SetupShoppingListMockInBasket()
            {
                var list = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
                ShoppingListMock = new ShoppingListMock(list);
            }

            public void SetupShoppingListMockNotInBasket()
            {
                var list = ShoppingListMother.OneSectionWithOneItemNotInBasket().Create();
                ShoppingListMock = new ShoppingListMock(list);
            }

            public void SetupOldItemFromShoppingListNotInBasket()
            {
                OldItem = ShoppingListMock.GetRandomItem(CommonFixture, i => !i.IsInBasket);
            }

            public void SetupOldItemFromShoppingListInBasket()
            {
                OldItem = ShoppingListMock.GetRandomItem(CommonFixture, i => i.IsInBasket);
            }

            public void SetupOldItem()
            {
                OldItem = ShoppingListItemMother.InBasket().Create();
            }

            public void SetupOldItemNull()
            {
                OldItem = null;
            }

            #region Mock Setup

            public void SetupFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindActiveByAsync(OldItem.Id, ShoppingListMock.Object.ToMonoList());
            }

            public void SetupFindingNoShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindActiveByAsync(OldItem.Id, Enumerable.Empty<IShoppingList>());
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyRemoveItemOnce()
            {
                ShoppingListMock.VerifyRemoveItemOnce(OldItem.Id);
            }

            public void VerifyStoreShoppingListOnce()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
            }

            public void VerifyStoreShoppingListNever()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyAddItemToShoppingListOnce()
            {
                var defaultSectionId = NewItem.Availabilities
                    .First(av => av.StoreId == ShoppingListMock.Object.StoreId)
                    .DefaultSectionId;

                AddItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(ShoppingListMock.Object,
                    NewItem.Id, defaultSectionId, OldItem.Quantity);
            }

            public void VerifyAddItemToShoppingListNever()
            {
                ShoppingListMock.VerifyAddItemNever();
            }

            public void VerifyPutItemInBasketNever()
            {
                ShoppingListMock.VerifyPutItemInBasketNever();
            }

            public void VerifyPutItemInBasketOnce()
            {
                ShoppingListMock.VerifyPutItemInBasketOnce(NewItem.Id);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithNewItemNotAvailableForShoppingList()
            {
                SetupShoppingListMockNotInBasket();
                SetupOldItemFromShoppingListNotInBasket();
                SetupNewItemNotMatchingShoppingList();
                SetupFindingShoppingList();
            }

            public void SetupWithNewItemAvailableForShoppingListAndInBasket()
            {
                SetupShoppingListMockInBasket();
                SetupOldItemFromShoppingListInBasket();
                SetupNewItemMatchingShoppingList();
                SetupFindingShoppingList();
            }

            public void SetupWithNewItemAvailableForShoppingListAndNotInBasket()
            {
                SetupShoppingListMockNotInBasket();
                SetupOldItemFromShoppingListNotInBasket();
                SetupNewItemMatchingShoppingList();
                SetupFindingShoppingList();
            }

            #endregion Aggregates
        }
    }
}