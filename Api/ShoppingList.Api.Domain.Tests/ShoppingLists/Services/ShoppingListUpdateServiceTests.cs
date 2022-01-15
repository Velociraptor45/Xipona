using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
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
        public class ExchangeItemAsyncTestsWithoutTypes
        {
            private readonly ExchangeItemAsyncWithoutTypesFixture _fixture;

            public ExchangeItemAsyncTestsWithoutTypes()
            {
                _fixture = new ExchangeItemAsyncWithoutTypesFixture();
            }

            #region ExchangeItemAsync

            [Fact]
            public async Task ExchangeItemAsync_WithOldItemIsNull_ShouldThrowArgumentNullException()
            {
                // Arrange
                var service = _fixture.CreateSut();
                _fixture.SetupNewItem();
                _fixture.SetupOldItemNull();

                // Act
                Func<Task> function = async () =>
                    await service.ExchangeItemAsync(_fixture.OldShoppingListItem?.Id, _fixture.NewItem, default);

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
                var service = _fixture.CreateSut();
                _fixture.SetupNewItemNull();
                _fixture.SetupOldItem();

                // Act
                Func<Task> function = async () =>
                    await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

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
                var service = _fixture.CreateSut();

                _fixture.SetupOldItem();
                _fixture.SetupNewItem();
                _fixture.SetupFindingNoShoppingList();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyStoreShoppingListNever();
                }
            }

            #region WithNewItemNotAvailableForShoppingList

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldRemoveOldItem()
            {
                // Arrange
                _fixture.SetupWithNewItemNotAvailableForShoppingList();

                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyRemoveItemOnce();
                }
            }

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldNotAddItemToShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemNotAvailableForShoppingList();

                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyAddItemToShoppingListNever();
                }
            }

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldStoreShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemNotAvailableForShoppingList();

                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyStoreShoppingListOnce();
                }
            }

            #endregion WithNewItemNotAvailableForShoppingList

            #region WithNewItemAvailableForShoppingListAndInBasket

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldRemoveOldItemFromShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyRemoveItemOnce();
                }
            }

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldStoreShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyStoreShoppingListOnce();
                }
            }

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldAddItemToShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyAddItemToShoppingListOnce();
                }
            }

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldPutItemInBasket()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyPutItemInBasketOnce();
                }
            }

            #endregion WithNewItemAvailableForShoppingListAndInBasket

            #region WithNewItemAvailableForShoppingListAndNotInBasket

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldRemoveOldItemFromShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyRemoveItemOnce();
                }
            }

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldStoreShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyStoreShoppingListOnce();
                }
            }

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldAddNewItemToShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyAddItemToShoppingListOnce();
                }
            }

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldNotPutItemInBasket()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyPutItemInBasketNever();
                }
            }

            #endregion WithNewItemAvailableForShoppingListAndNotInBasket

            #endregion ExchangeItemAsync

            private class ExchangeItemAsyncWithoutTypesFixture : ExchangeItemAsyncFixture
            {
                public override void SetupNewItem()
                {
                    NewItem = StoreItemMother.Initial().Create();
                }

                protected override void SetupNewItemForStore(StoreId storeId)
                {
                    var availability = StoreItemAvailabilityMother.Initial()
                        .WithStoreId(storeId)
                        .Create();
                    NewItem = StoreItemMother.Initial().WithAvailability(availability).Create();
                }

                public override void SetupShoppingListMockWithItemInBasket()
                {
                    var list = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
                    ShoppingListMock = new ShoppingListMock(list);
                }

                public override void SetupShoppingListWithItemNotInBasket()
                {
                    var list = ShoppingListMother.OneSectionWithOneItemNotInBasket().Create();
                    ShoppingListMock = new ShoppingListMock(list);
                }

                public override void SetupOldItem()
                {
                    OldShoppingListItem = ShoppingListItemMother.InBasket().Create();
                }

                public override void SetupAddingItemToShoppingList()
                {
                    var sectionId = NewItem.GetDefaultSectionIdForStore(ShoppingListMock.Object.StoreId);
                    AddItemToShoppingListServiceMock.SetupAddItemToShoppingList(ShoppingListMock.Object, NewItem.Id,
                        sectionId, OldShoppingListItem.Quantity);
                }

                #region Verify

                public override void VerifyRemoveItemOnce()
                {
                    ShoppingListMock.VerifyRemoveItemOnce(OldShoppingListItem.Id);
                }

                public override void VerifyAddItemToShoppingListOnce()
                {
                    var defaultSectionId = NewItem.Availabilities
                        .First(av => av.StoreId == ShoppingListMock.Object.StoreId)
                        .DefaultSectionId;

                    AddItemToShoppingListServiceMock.VerifyAddItemToShoppingListOnce(ShoppingListMock.Object,
                        NewItem.Id, defaultSectionId, OldShoppingListItem.Quantity);
                }

                public override void VerifyPutItemInBasketNever()
                {
                    ShoppingListMock.VerifyPutItemInBasketNever();
                }

                public override void VerifyPutItemInBasketOnce()
                {
                    ShoppingListMock.VerifyPutItemInBasketOnce(NewItem.Id);
                }

                #endregion Verify

                #region Aggregates

                public void SetupWithNewItemNotAvailableForShoppingList()
                {
                    SetupShoppingListWithItemNotInBasket();
                    SetupOldItemFromShoppingListNotInBasket();
                    SetupNewItemNotMatchingShoppingList();
                    SetupFindingShoppingList();
                    SetupStoringShoppingList();
                }

                public void SetupWithNewItemAvailableForShoppingListAndInBasket()
                {
                    SetupShoppingListMockWithItemInBasket();
                    SetupOldItemFromShoppingListInBasket();
                    SetupNewItemMatchingShoppingList();
                    SetupFindingShoppingList();
                    SetupAddingItemToShoppingList();
                    SetupStoringShoppingList();
                }

                public void SetupWithNewItemAvailableForShoppingListAndNotInBasket()
                {
                    SetupShoppingListWithItemNotInBasket();
                    SetupOldItemFromShoppingListNotInBasket();
                    SetupNewItemMatchingShoppingList();
                    SetupFindingShoppingList();
                    SetupAddingItemToShoppingList();
                    SetupStoringShoppingList();
                }

                #endregion Aggregates
            }
        }

        public class ExchangeItemAsyncTestsWithTypes
        {
            private readonly ExchangeItemAsyncWithTypesFixture _fixture;

            public ExchangeItemAsyncTestsWithTypes()
            {
                _fixture = new ExchangeItemAsyncWithTypesFixture();
            }

            #region WithNewItemTypeNotAvailableForShoppingList

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldRemoveOldItem()
            {
                // Arrange
                _fixture.SetupWithNewItemNotAvailableForShoppingList();

                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyRemoveItemOnce();
                }
            }

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldNotAddItemToShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemNotAvailableForShoppingList();

                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyAddItemToShoppingListNever();
                }
            }

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldStoreShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemNotAvailableForShoppingList();

                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyStoreShoppingListOnce();
                }
            }

            #endregion WithNewItemTypeNotAvailableForShoppingList

            #region WithNewItemAvailableForShoppingListAndInBasket

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldRemoveOldItemFromShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyRemoveItemOnce();
                }
            }

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldStoreShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyStoreShoppingListOnce();
                }
            }

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldAddItemToShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyAddItemToShoppingListOnce();
                }
            }

            [Fact]
            public async Task ExchangeItemAsync_WithNewItemAvailableForShoppingListAndInBasket_ShouldPutItemInBasket()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyPutItemInBasketOnce();
                }
            }

            #endregion WithNewItemAvailableForShoppingListAndInBasket

            #region WithNewItemAvailableForShoppingListAndNotInBasket

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldRemoveOldItemFromShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyRemoveItemOnce();
                }
            }

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldStoreShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyStoreShoppingListOnce();
                }
            }

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldAddNewItemToShoppingList()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyAddItemToShoppingListOnce();
                }
            }

            [Fact]
            public async Task
                ExchangeItemAsync_WithNewItemAvailableForShoppingListAndNotInBasket_ShouldNotPutItemInBasket()
            {
                // Arrange
                _fixture.SetupWithNewItemAvailableForShoppingListAndNotInBasket();
                var service = _fixture.CreateSut();

                // Act
                await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem, default);

                // Assert
                using (new AssertionScope())
                {
                    _fixture.VerifyPutItemInBasketNever();
                }
            }

            #endregion WithNewItemAvailableForShoppingListAndNotInBasket

            private class ExchangeItemAsyncWithTypesFixture : ExchangeItemAsyncFixture
            {
                public override void SetupNewItem()
                {
                    NewItem = StoreItemMother.InitialWithTypes().Create();
                }

                protected override void SetupNewItemForStore(StoreId storeId)
                {
                    var availability = StoreItemAvailabilityMother.Initial()
                        .WithStoreId(storeId)
                        .CreateMany(1);
                    var type = new ItemTypeBuilder().WithAvailabilities(availability).CreateMany(1);
                    type.First().SetPredecessor(new ItemTypeBuilder().WithId(OldShoppingListItem.TypeId).Create());
                    NewItem = new StoreItemBuilder().WithTypes(type).Create();
                }

                public override void SetupShoppingListMockWithItemInBasket()
                {
                    var items = ShoppingListItemMother.InBasket().CreateMany(1).ToList();
                    items.AddRange(ShoppingListItemMother.NotInBasket().CreateMany(2));
                    var list = ShoppingListMother.OneSection(items).Create();
                    ShoppingListMock = new ShoppingListMock(list);
                }

                public override void SetupShoppingListWithItemNotInBasket()
                {
                    var items = ShoppingListItemMother.NotInBasket().CreateMany(1).ToList();
                    items.AddRange(ShoppingListItemMother.InBasket().CreateMany(2));
                    var list = ShoppingListMother.OneSection(items).Create();
                    ShoppingListMock = new ShoppingListMock(list);
                }

                public override void SetupOldItem()
                {
                    throw new NotImplementedException();
                }

                public override void SetupAddingItemToShoppingList()
                {
                    var type = NewItem.ItemTypes.First();
                    var sectionId = type.GetDefaultSectionIdForStore(ShoppingListMock.Object.StoreId);
                    AddItemToShoppingListServiceMock.SetupAddItemWithTypeToShoppingList(ShoppingListMock.Object, NewItem,
                        type.Id, sectionId, OldShoppingListItem.Quantity);
                }

                #region Verify

                public override void VerifyRemoveItemOnce()
                {
                    ShoppingListMock.VerifyRemoveItemOnce(OldShoppingListItem.Id, OldShoppingListItem.TypeId);
                }

                public override void VerifyAddItemToShoppingListOnce()
                {
                    var type = NewItem.ItemTypes.First();
                    var sectionId = type.GetDefaultSectionIdForStore(ShoppingListMock.Object.StoreId);
                    AddItemToShoppingListServiceMock.VerifyAddItemWithTypeToShoppingList(ShoppingListMock.Object, NewItem,
                        type.Id, sectionId, OldShoppingListItem.Quantity, Times.Once);
                }

                public override void VerifyPutItemInBasketNever()
                {
                    ShoppingListMock.VerifyPutItemInBasketWithTypeIdNever();
                }

                public override void VerifyPutItemInBasketOnce()
                {
                    ShoppingListMock.VerifyPutItemInBasket(NewItem.Id, NewItem.ItemTypes.First().Id, Times.Once);
                }

                #endregion Verify

                #region Aggregates

                public void SetupWithNewItemNotAvailableForShoppingList()
                {
                    SetupShoppingListWithItemNotInBasket();
                    SetupOldItemFromShoppingListNotInBasket();
                    SetupNewItemNotMatchingShoppingList();
                    SetupFindingShoppingList();
                    SetupStoringShoppingList();
                }

                public void SetupWithNewItemAvailableForShoppingListAndInBasket()
                {
                    SetupShoppingListMockWithItemInBasket();
                    SetupOldItemFromShoppingListInBasket();
                    SetupNewItemMatchingShoppingList();
                    SetupFindingShoppingList();
                    SetupAddingItemToShoppingList();
                    SetupStoringShoppingList();
                }

                public void SetupWithNewItemAvailableForShoppingListAndNotInBasket()
                {
                    SetupShoppingListWithItemNotInBasket();
                    SetupOldItemFromShoppingListNotInBasket();
                    SetupNewItemMatchingShoppingList();
                    SetupFindingShoppingList();
                    SetupAddingItemToShoppingList();
                    SetupStoringShoppingList();
                }

                #endregion Aggregates
            }
        }

        public abstract class ExchangeItemAsyncFixture : LocalFixture
        {
            protected ShoppingListMock ShoppingListMock;
            public IStoreItem NewItem { get; protected set; }
            public IShoppingListItem OldShoppingListItem { get; protected set; }

            public abstract void SetupNewItem();

            public void SetupNewItemNull()
            {
                NewItem = null;
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

            protected abstract void SetupNewItemForStore(StoreId storeId);

            public abstract void SetupShoppingListMockWithItemInBasket();

            public abstract void SetupShoppingListWithItemNotInBasket();

            public void SetupOldItemFromShoppingListNotInBasket()
            {
                OldShoppingListItem = ShoppingListMock.GetRandomItem(CommonFixture, i => !i.IsInBasket);
            }

            public void SetupOldItemFromShoppingListInBasket()
            {
                OldShoppingListItem = ShoppingListMock.GetRandomItem(CommonFixture, i => i.IsInBasket);
            }

            public abstract void SetupOldItem();

            public void SetupOldItemNull()
            {
                OldShoppingListItem = null;
            }

            #region Mock Setup

            public void SetupFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindActiveByAsync(OldShoppingListItem.Id, ShoppingListMock.Object.ToMonoList());
            }

            public void SetupFindingNoShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindActiveByAsync(OldShoppingListItem.Id, Enumerable.Empty<IShoppingList>());
            }

            public abstract void SetupAddingItemToShoppingList();

            public void SetupStoringShoppingList()
            {
                ShoppingListRepositoryMock.SetupStoreAsync(ShoppingListMock.Object);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyStoreShoppingListOnce()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
            }

            public void VerifyStoreShoppingListNever()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncNever();
            }

            public abstract void VerifyRemoveItemOnce();

            public abstract void VerifyAddItemToShoppingListOnce();

            public void VerifyAddItemToShoppingListNever()
            {
                ShoppingListMock.VerifyAddItemNever();
            }

            public abstract void VerifyPutItemInBasketNever();

            public abstract void VerifyPutItemInBasketOnce();

            #endregion Verify
        }

        public abstract class LocalFixture
        {
            protected CommonFixture CommonFixture = new CommonFixture();
            protected ShoppingListRepositoryMock ShoppingListRepositoryMock;
            protected AddItemToShoppingListServiceMock AddItemToShoppingListServiceMock;

            protected LocalFixture()
            {
                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
                AddItemToShoppingListServiceMock = new AddItemToShoppingListServiceMock(MockBehavior.Strict);
            }

            public ShoppingListUpdateService CreateSut()
            {
                return new ShoppingListUpdateService(
                    ShoppingListRepositoryMock.Object,
                    AddItemToShoppingListServiceMock.Object);
            }
        }
    }
}