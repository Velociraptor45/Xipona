﻿using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Exchanges;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using Xunit.Abstractions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ShoppingLists.Services;

public class ShoppingListExchangeServiceTests
{
    public class ExchangeItemAsyncTestsWithoutTypes
    {
        private readonly ExchangeItemAsyncWithoutTypesFixture _fixture;

        public ExchangeItemAsyncTestsWithoutTypes(ITestOutputHelper output)
        {
            _fixture = new ExchangeItemAsyncWithoutTypesFixture(output);
        }

        #region ExchangeItemAsync

        [Fact]
        public async Task ExchangeItemAsync_WithOldItemOnNoShoppingLists_ShouldDoNothing()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupOldItem();
            _fixture.SetupNewItem();
            _fixture.SetupFindingNoShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoreShoppingListNever();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithShoppingListItemHasType_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupShoppingListWithItemWithType();
            _fixture.SetupOldItemFromShoppingListInBasket();
            _fixture.SetupNewItemMatchingShoppingList();
            _fixture.SetupFindingShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            Func<Task> func = async () =>
                await sut.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListItemHasType);
        }

        #region WithNewItemNotAvailableForShoppingList

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldRemoveOldItem()
        {
            // Arrange
            _fixture.SetupWithNewItemNotAvailableForShoppingList();

            var service = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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
            public ExchangeItemAsyncWithoutTypesFixture(ITestOutputHelper output) : base(output)
            {
            }

            public void SetupNewItem()
            {
                NewItem = ItemMother.Initial().Create();
            }

            protected override void SetupNewItemForStore(StoreId storeId)
            {
                var availability = ItemAvailabilityMother.Initial()
                    .WithStoreId(storeId)
                    .Create();
                NewItem = ItemMother.Initial().WithAvailability(availability).Create();
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

            public void SetupShoppingListWithItemWithType()
            {
                var items = ShoppingListItemMother.InBasket().CreateMany(1).ToList();
                items.AddRange(ShoppingListItemMother.NotInBasket().CreateMany(2));
                var list = ShoppingListMother.OneSection(items).Create();
                ShoppingListMock = new ShoppingListMock(list);
            }

            public void SetupOldItem()
            {
                OldShoppingListItem = ShoppingListItemMother.InBasket().Create();
            }

            public override void SetupAddingItemToShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                var sectionId = NewItem.GetDefaultSectionIdForStore(ShoppingListMock.Object.StoreId);
                AddItemToShoppingListServiceMock.SetupAddItemAsync(ShoppingListMock.Object, NewItem.Id,
                    sectionId, OldShoppingListItem.Quantity);
            }

            #region Verify

            public override void VerifyRemoveItemOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
                ShoppingListMock.VerifyRemoveItemOnce(OldShoppingListItem.Id);
            }

            public override void VerifyAddItemToShoppingListOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                var defaultSectionId = NewItem.Availabilities
                    .First(av => av.StoreId == ShoppingListMock.Object.StoreId)
                    .DefaultSectionId;

                AddItemToShoppingListServiceMock.VerifyAddItemAsyncOnce(ShoppingListMock.Object,
                    NewItem.Id, defaultSectionId, OldShoppingListItem.Quantity);
            }

            public override void VerifyPutItemInBasketNever()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                ShoppingListMock.VerifyPutItemInBasketNever();
            }

            public override void VerifyPutItemInBasketOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
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

        public ExchangeItemAsyncTestsWithTypes(ITestOutputHelper output)
        {
            _fixture = new ExchangeItemAsyncWithTypesFixture(output);
        }

        [Fact]
        public async Task ExchangeItemAsync_WithShoppingListItemHasNoType_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupShoppingListWithItemWithoutType();
            _fixture.SetupOldItemFromShoppingListInBasket();
            _fixture.SetupItemMatchingShoppingListWithNewTypes();
            _fixture.SetupFindingShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            Func<Task> func = async () =>
                await sut.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListItemHasNoType);
        }

        #region WithItemTypeRemoved

        [Fact]
        public async Task ExchangeItemAsync_WithItemTypeRemoved_ShouldRemoveOldItem()
        {
            // Arrange
            _fixture.SetupWithItemTypeRemoved();

            var service = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyRemoveItemOnce();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithItemTypeRemoved_ShouldNotAddItemToShoppingList()
        {
            // Arrange
            _fixture.SetupWithItemTypeRemoved();

            var service = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyAddItemToShoppingListNever();
            }
        }

        [Fact]
        public async Task ExchangeItemAsync_WithItemTypeRemoved_ShouldStoreShoppingList()
        {
            // Arrange
            _fixture.SetupWithItemTypeRemoved();

            var service = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoreShoppingListOnce();
            }
        }

        #endregion WithItemTypeRemoved

        #region WithNewItemTypeNotAvailableForShoppingList

        [Fact]
        public async Task ExchangeItemAsync_WithNewItemNotAvailableForShoppingList_ShouldRemoveOldItem()
        {
            // Arrange
            _fixture.SetupWithNewItemNotAvailableForShoppingList();

            var service = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldShoppingListItem);

            // Act
            await service.ExchangeItemAsync(_fixture.OldShoppingListItem.Id, _fixture.NewItem);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyPutItemInBasketNever();
            }
        }

        #endregion WithNewItemAvailableForShoppingListAndNotInBasket

        private class ExchangeItemAsyncWithTypesFixture : ExchangeItemAsyncFixture
        {
            private readonly ItemTypeFactoryMock _itemTypeFactoryMock;

            public ExchangeItemAsyncWithTypesFixture(ITestOutputHelper output) : base(output)
            {
                _itemTypeFactoryMock = new ItemTypeFactoryMock(MockBehavior.Strict);
            }

            protected override void SetupNewItemForStore(StoreId storeId)
            {
                TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
                TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem.TypeId);

                ExpectedNewSection = SectionId.New;

                var availability = ItemAvailabilityMother.Initial()
                    .WithStoreId(storeId)
                    .WithDefaultSectionId(ExpectedNewSection.Value)
                    .CreateMany(1);
                var type = new ItemTypeBuilder()
                    .WithAvailabilities(availability)
                    .WithPredecessorId(OldShoppingListItem.TypeId.Value)
                    .CreateMany(1).ToList();
                var itemTypes = new ItemTypes(type, _itemTypeFactoryMock.Object);
                NewItem = new ItemBuilder().WithTypes(itemTypes).Create();
            }

            public void SetupItemMatchingShoppingListWithNewTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                var availability = ItemAvailabilityMother.Initial()
                    .WithStoreId(ShoppingListMock.Object.StoreId)
                    .CreateMany(1);
                var type = new ItemTypeBuilder().WithAvailabilities(availability).CreateMany(1);
                var itemTypes = new ItemTypes(type, _itemTypeFactoryMock.Object);
                NewItem = new ItemBuilder().WithTypes(itemTypes).Create();
            }

            public void SetupShoppingListWithItemWithoutType()
            {
                var list = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
                ShoppingListMock = new ShoppingListMock(list);
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

            public override void SetupAddingItemToShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewSection);
                var type = NewItem.ItemTypes.First();
                AddItemToShoppingListServiceMock.SetupAddItemWithTypeAsync(ShoppingListMock.Object, NewItem,
                    type.Id, ExpectedNewSection.Value, OldShoppingListItem.Quantity);
            }

            #region Verify

            public override void VerifyRemoveItemOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                ShoppingListMock.VerifyRemoveItem(OldShoppingListItem.Id, OldShoppingListItem.TypeId, Times.Once);
            }

            public override void VerifyAddItemToShoppingListOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(ExpectedNewSection);
                var type = NewItem.ItemTypes.First();
                AddItemToShoppingListServiceMock.VerifyAddItemWithTypeAsync(ShoppingListMock.Object, NewItem,
                    type.Id, ExpectedNewSection.Value, OldShoppingListItem.Quantity, Times.Once);
            }

            public override void VerifyPutItemInBasketNever()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                ShoppingListMock.VerifyPutItemInBasketWithTypeIdNever();
            }

            public override void VerifyPutItemInBasketOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                ShoppingListMock.VerifyPutItemInBasket(NewItem.Id, NewItem.ItemTypes.First().Id, Times.Once);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithItemTypeRemoved()
            {
                SetupShoppingListWithItemNotInBasket();
                SetupOldItemFromShoppingListNotInBasket();
                SetupItemMatchingShoppingListWithNewTypes();
                SetupFindingShoppingList();
                SetupStoringShoppingList();
            }

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

    public abstract class ExchangeItemAsyncFixture : ShoppingListExchangeServiceFixture
    {
        protected ShoppingListMock? ShoppingListMock;
        protected SectionId? ExpectedNewSection;

        protected ExchangeItemAsyncFixture(ITestOutputHelper output) : base(output)
        {
        }

        public IItem? NewItem { get; protected set; }
        public ShoppingListItem? OldShoppingListItem { get; protected set; }

        public void SetupNewItemMatchingShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            SetupNewItemForStore(ShoppingListMock.Object.StoreId);
        }

        public void SetupNewItemNotMatchingShoppingList()
        {
            var storeId = StoreId.New;
            SetupNewItemForStore(storeId);
        }

        protected abstract void SetupNewItemForStore(StoreId storeId);

        public abstract void SetupShoppingListMockWithItemInBasket();

        public abstract void SetupShoppingListWithItemNotInBasket();

        public void SetupOldItemFromShoppingListNotInBasket()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            OldShoppingListItem = ShoppingListMock.GetRandomItem(CommonFixture, i => !i.IsInBasket);
        }

        public void SetupOldItemFromShoppingListInBasket()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            OldShoppingListItem = ShoppingListMock.GetRandomItem(CommonFixture, i => i.IsInBasket);
        }

        #region Mock Setup

        public void SetupFindingShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            ShoppingListRepositoryMock.SetupFindActiveByAsync(OldShoppingListItem.Id, ShoppingListMock.Object.ToMonoList());
        }

        public void SetupFindingNoShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(OldShoppingListItem);
            ShoppingListRepositoryMock.SetupFindActiveByAsync(OldShoppingListItem.Id, Enumerable.Empty<IShoppingList>());
        }

        public abstract void SetupAddingItemToShoppingList();

        public void SetupStoringShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            ShoppingListRepositoryMock.SetupStoreAsync(ShoppingListMock.Object);
        }

        #endregion Mock Setup

        #region Verify

        public void VerifyStoreShoppingListOnce()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
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
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            ShoppingListMock.VerifyAddItemNever();
        }

        public abstract void VerifyPutItemInBasketNever();

        public abstract void VerifyPutItemInBasketOnce();

        #endregion Verify
    }

    public abstract class ShoppingListExchangeServiceFixture
    {
        protected CommonFixture CommonFixture = new();
        protected ShoppingListRepositoryMock ShoppingListRepositoryMock;
        protected AddItemToShoppingListServiceMock AddItemToShoppingListServiceMock;
        private readonly ILogger<ShoppingListExchangeService> _logger;

        protected ShoppingListExchangeServiceFixture(ITestOutputHelper output)
        {
            ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            AddItemToShoppingListServiceMock = new AddItemToShoppingListServiceMock(MockBehavior.Strict);
            _logger = output.BuildLoggerFor<ShoppingListExchangeService>();
        }

        public ShoppingListExchangeService CreateSut()
        {
            return new ShoppingListExchangeService(
                ShoppingListRepositoryMock.Object,
                AddItemToShoppingListServiceMock.Object,
                _logger);
        }
    }
}