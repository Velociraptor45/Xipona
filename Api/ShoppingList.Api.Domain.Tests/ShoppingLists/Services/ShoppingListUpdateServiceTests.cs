using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services
{
    public class ShoppingListUpdateServiceTests
    {
        #region ExchangeItemAsync

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();
            var newItem = local.CreateNewItem();

            // Act
            Func<Task> function = async () => await service.ExchangeItemAsync(null, newItem, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithOldItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();
            var oldItemId = local.CreateOldItemId();

            // Act
            Func<Task> function = async () => await service.ExchangeItemAsync(oldItemId, null, default);

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
            var local = new LocalFixture();
            var service = local.CreateService();

            ItemId oldItemId = local.CreateOldItemId();
            IStoreItem newItem = local.CreateNewItem();

            local.ShoppingListRepositoryMock.SetupFindActiveByAsync(oldItemId, Enumerable.Empty<IShoppingList>());

            // Act
            await service.ExchangeItemAsync(oldItemId, newItem, default);

            // Assert
            using (new AssertionScope())
            {
                local.ShoppingListRepositoryMock.VerifyStoreAsyncNever();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldRemoveOldItemAndNotAddNewItem()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            ShoppingListMock shoppingListMock = local.CreateShoppingListMockInBasket();
            ItemId oldItemId = local.CommonFixture.ChooseRandom(shoppingListMock.Object.Items).Id;
            IStoreItem newItem = local.CreateNewItemNotInStore(shoppingListMock.Object.StoreId);

            local.ShoppingListRepositoryMock.SetupFindActiveByAsync(oldItemId, shoppingListMock.Object.ToMonoList());

            // Act
            await service.ExchangeItemAsync(oldItemId, newItem, default);

            // Assert
            using (new AssertionScope())
            {
                shoppingListMock.VerifyRemoveItemOnce(oldItemId);
                shoppingListMock.VerifyAddItemNever();
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldRemoveOldItemAndAddNewItem()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            ShoppingListMock shoppingListMock = local.CreateShoppingListMockInBasket();
            var shopppingListStoreId = shoppingListMock.Object.StoreId;

            IShoppingListItem oldShoppingListItem = shoppingListMock.GetRandomItem(local.CommonFixture, i => i.IsInBasket);
            ItemId oldItemId = oldShoppingListItem.Id;

            IStoreItem newItem = local.CreateNewItemForStore(shopppingListStoreId);

            local.ShoppingListRepositoryMock.SetupFindActiveByAsync(oldItemId, shoppingListMock.Object.ToMonoList());

            // Act
            await service.ExchangeItemAsync(oldItemId, newItem, default);

            // Assert
            var sectionId = newItem.Availabilities.First(av => av.StoreId == shopppingListStoreId).DefaultSectionId;

            using (new AssertionScope())
            {
                shoppingListMock.VerifyRemoveItemOnce(oldItemId);
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
                local.AddItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(shoppingListMock.Object,
                    newItem.Id, sectionId, oldShoppingListItem.Quantity);
                shoppingListMock.VerifyPutItemInBasketOnce(newItem.Id);
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldRemoveOldItemAndAddNewItem()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            ShoppingListMock shoppingListMock = local.CreateShoppingListMockNotInBasket();
            var shopppingListStoreId = shoppingListMock.Object.StoreId;

            IShoppingListItem oldShoppingListItem = shoppingListMock.GetRandomItem(local.CommonFixture, i => !i.IsInBasket);
            ItemId oldItemId = oldShoppingListItem.Id;

            IStoreItem newItem = local.CreateNewItemForStore(shopppingListStoreId);

            local.ShoppingListRepositoryMock.SetupFindActiveByAsync(oldItemId, shoppingListMock.Object.ToMonoList());

            // Act
            await service.ExchangeItemAsync(oldItemId, newItem, default);

            // Assert
            var sectionId = newItem.Availabilities.First(av => av.StoreId == shopppingListStoreId).DefaultSectionId;

            using (new AssertionScope())
            {
                shoppingListMock.VerifyRemoveItemOnce(oldItemId);
                local.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(shoppingListMock.Object);
                local.AddItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(shoppingListMock.Object,
                    newItem.Id, sectionId, oldShoppingListItem.Quantity);
                shoppingListMock.VerifyPutItemInBasketNever();
            }
        }

        #endregion ExchangeItemAsync

        private class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListItemFixture ShoppingListItemFixture { get; }
            public ShoppingListFixture ShoppingListFixture { get; }
            public ShoppingListMockFixture ShoppingListMockFixture { get; }
            public StoreItemAvailabilityFixture StoreItemAvailabilityFixture { get; }
            public StoreItemFixture StoreItemFixture { get; }
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public AddItemToShoppingListServiceMock AddItemToShoppingListServiceMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListItemFixture = new ShoppingListItemFixture(CommonFixture);
                ShoppingListFixture = new ShoppingListFixture(CommonFixture);
                ShoppingListMockFixture = new ShoppingListMockFixture(CommonFixture, ShoppingListFixture);
                StoreItemAvailabilityFixture = new StoreItemAvailabilityFixture(CommonFixture);
                StoreItemFixture = new StoreItemFixture(StoreItemAvailabilityFixture, CommonFixture);

                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(Fixture);
                AddItemToShoppingListServiceMock = new AddItemToShoppingListServiceMock(Fixture);
            }

            public ShoppingListUpdateService CreateService()
            {
                return Fixture.Create<ShoppingListUpdateService>();
            }

            public ItemId CreateOldItemId()
            {
                return new ItemId(CommonFixture.NextInt());
            }

            public IStoreItem CreateNewItem()
            {
                return StoreItemFixture.CreateValid();
            }

            public IStoreItem CreateNewItemForStore(StoreId storeId)
            {
                var availabilityDef = StoreItemAvailabilityDefinition.FromStoreId(storeId);
                var availability = StoreItemAvailabilityFixture.Create(availabilityDef);

                var storeItemDef = new StoreItemDefinition
                {
                    Availabilities = availability.ToMonoList()
                };

                return StoreItemFixture.Create(storeItemDef);
            }

            public IStoreItem CreateNewItemNotInStore(StoreId storeId)
            {
                var availabilityStoreId = new StoreId(CommonFixture.NextInt(storeId.Value));
                return CreateNewItemForStore(availabilityStoreId);
            }

            public IShoppingListItem CreateShoppingListItem()
            {
                return ShoppingListItemFixture.AsModelFixture().CreateValid();
            }

            public ShoppingListMock CreateShoppingListMockInBasket()
            {
                var itemDef = ShoppingListItemDefinition.FromIsInBasket(true);
                var list = ShoppingListFixture.CreateValidWith(itemDef);

                return new ShoppingListMock(list);
            }

            public ShoppingListMock CreateShoppingListMockNotInBasket()
            {
                var itemDef = ShoppingListItemDefinition.FromIsInBasket(false);
                var list = ShoppingListFixture.CreateValidWith(itemDef);

                return new ShoppingListMock(list);
            }
        }
    }
}