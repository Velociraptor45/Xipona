﻿using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services;

public class AddItemToShoppingListServiceTests
{
    public class AddItemToShoppingListTests
    {
        private readonly AddItemToShoppingListFixture _fixture;

        public AddItemToShoppingListTests()
        {
            _fixture = new AddItemToShoppingListFixture();
        }

        #region ItemId

        [Fact]
        public async Task AddItemToShoppingList_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupShoppingListMock();
            _fixture.SetupStoreItem();
            _fixture.SetupSectionId();
            _fixture.SetupQuantity();

            _fixture.SetupNotFindingItem();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingListAsync(
                _fixture.ShoppingListMock.Object, _fixture.StoreItem.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithItemIdAndValidData_ShouldAddItemToShoppingList()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupStoreItem();
            _fixture.SetupShoppingListMockMatchingStoreItem();
            _fixture.SetupShoppingListItem();

            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupStore();
            _fixture.SetupQuantity();

            _fixture.SetupFindingItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupFindingStore();

            _fixture.SetupAddingItemToShoppingList();

            // Act
            await service.AddItemToShoppingListAsync(
                _fixture.ShoppingListMock.Object, _fixture.StoreItem.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyAddSectionNever();
                _fixture.VerifyAddItemOnce();
            }
        }

        #endregion ItemId

        #region TemporaryItemId

        [Fact]
        public async Task AddItemToShoppingList_WithInvalidTemporaryItemId_ShouldThrowDomainException()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupShoppingListMock();
            _fixture.SetupTemporaryStoreItem();
            _fixture.SetupSectionId();
            _fixture.SetupQuantity();

            _fixture.SetupFindingNoItemWithTemporaryId();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.StoreItem.TemporaryId.Value, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithTemporaryItemIdAndValidData_ShouldAddItemToShoppingList()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupTemporaryStoreItem();
            _fixture.SetupShoppingListMockMatchingStoreItem();
            _fixture.SetupShoppingListItem();

            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupStore();
            _fixture.SetupQuantity();

            _fixture.SetupFindingItemWithTemporaryId();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupFindingStore();
            _fixture.SetupAddingItemToShoppingList();

            // Act
            await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.StoreItem.TemporaryId.Value, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyAddSectionNever();
                _fixture.VerifyAddItemOnce();
            }
        }

        #endregion TemporaryItemId

        #region AddItemToShoppingList (internal 1st)

        [Fact]
        public async Task AddItemToShoppingList_WithItemAtStoreNotAvailable_ShouldThrowDomainException()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupStoreItem();
            _fixture.SetupShoppingListMockNotMatchingStoreItem();

            _fixture.SetupSectionId();
            _fixture.SetupQuantity();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.StoreItem!, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemAtStoreNotAvailable);
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithSectionIdIsNull_ShouldSetDefaultSectionId()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupStoreItem();
            _fixture.SetupShoppingListMockMatchingStoreItem();
            _fixture.SetupShoppingListItem();

            _fixture.SetupStore();
            _fixture.SetupQuantity();

            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupFindingStore();

            _fixture.SetupEmptyShoppingListSection();
            _fixture.SetupCreatingEmptyShoppingListSection();
            _fixture.SetupAddingSectionToShoppingList();
            _fixture.SetupAddingItemToShoppingList();

            // Act
            await service.AddItemToShoppingListAsync(
                _fixture.ShoppingListMock.Object, _fixture.StoreItem!, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyAddItemOnce();
            }
        }

        #endregion AddItemToShoppingList (internal 1st)

        #region AddItemToShoppingList (internal 2nd)

        [Fact]
        public async Task AddItemToShoppingList_WithStoreIdIsInvalid_ShouldThrowDomainException()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupShoppingListMock();
            _fixture.SetupShoppingListItem();
            _fixture.SetupSectionId();

            _fixture.SetupNotFindingStore();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.ShoppingListItem, _fixture.SectionId.Value, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithSectionIdNotInStore_ShouldThrowDomainException()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupShoppingListMock();
            _fixture.SetupShoppingListItem();
            _fixture.SetupSectionId();
            _fixture.SetupStoreNotMatchingSectionId();

            _fixture.SetupFindingStore();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.ShoppingListItem, _fixture.SectionId.Value, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.SectionInStoreNotFound);
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithSectionNotInShoppingList_AddSectionToShoppingList()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupShoppingListMock();
            _fixture.SetupShoppingListItem();
            _fixture.SetupSectionId();
            _fixture.SetupStore();
            _fixture.SetupEmptyShoppingListSection();

            _fixture.SetupFindingStore();
            _fixture.SetupCreatingEmptyShoppingListSection();
            _fixture.SetupAddingSectionToShoppingList();
            _fixture.SetupAddingItemToShoppingList();

            // Act
            await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.ShoppingListItem, _fixture.SectionId.Value, default);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyCreatingEmptyShoppingListSectionOnce();
                _fixture.VerifyAddSectionOnce();
                _fixture.VerifyAddItemOnce();
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithSectionAlreadyInShoppingList_ShouldNotAddSectionToShoppingList()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupShoppingListMock();
            _fixture.SetupShoppingListItem();
            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupStore();

            _fixture.SetupFindingStore();
            _fixture.SetupAddingItemToShoppingList();

            // Act
            await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.ShoppingListItem, _fixture.SectionId.Value, default);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyAddSectionNever();
                _fixture.VerifyAddItemOnce();
            }
        }

        #endregion AddItemToShoppingList (internal 2nd)

        private sealed class AddItemToShoppingListFixture : LocalFixture
        {
            public void SetupShoppingListMockMatchingStoreItem()
            {
                Availability = CommonFixture.ChooseRandom(StoreItem.Availabilities);

                var list = ShoppingListMother.Sections(3).WithStoreId(Availability.StoreId).Create();
                ShoppingListMock = new ShoppingListMock(list, MockBehavior.Strict);
            }

            public void SetupShoppingListMockNotMatchingStoreItem()
            {
                var storeId = new StoreIdBuilder().Create();

                var list = ShoppingListMother.Sections(3).WithStoreId(storeId).Create();
                ShoppingListMock = new ShoppingListMock(list, MockBehavior.Strict);
            }

            public void SetupShoppingListItem()
            {
                ShoppingListItem = ShoppingListItemMother.InBasket().WithoutTypeId().Create();
            }

            public void SetupStoreItem()
            {
                StoreItem = StoreItemMother.Initial().Create();
            }

            public void SetupStoreItemNull()
            {
                StoreItem = null;
            }

            public void SetupTemporaryStoreItem()
            {
                StoreItem = StoreItemMother.InitialTemporary().Create();
            }

            public void SetupTemporaryStoreItemNull()
            {
                StoreItem = null;
            }

            #region Mock Setup

            public void SetupCreatingShoppingListItem()
            {
                ShoppingListItemFactoryMock.SetupCreate(StoreItem.Id, null, false, Quantity, ShoppingListItem);
            }

            public void SetupFindingNoItemWithTemporaryId()
            {
                ItemRepositoryMock.SetupFindByAsync(StoreItem.TemporaryId.Value, null);
            }

            public void SetupFindingItemWithTemporaryId()
            {
                ItemRepositoryMock.SetupFindByAsync(StoreItem.TemporaryId.Value, StoreItem);
            }

            #endregion Mock Setup
        }
    }

    public class AddItemWithTypeToShoppingListTests
    {
        private readonly AddItemWithTypeToShoppingListFixture _fixture;

        public AddItemWithTypeToShoppingListTests()
        {
            _fixture = new AddItemWithTypeToShoppingListFixture();
        }

        #region Item

        #region WithMissingSection

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithMissingSection_ShouldAddItemToShoppingList()
        {
            // Arrange
            _fixture.SetupWithMissingSection();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyAddItemOnce();
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithMissingSection_ShouldAddSectionToShoppingList()
        {
            // Arrange
            _fixture.SetupWithMissingSection();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyAddSectionOnce();
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithMissingSection_ShouldStoreShoppingList()
        {
            // Arrange
            _fixture.SetupWithMissingSection();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyStoringShoppingList();
        }

        #endregion WithMissingSection

        #region WithExistingSection

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithExistingSection_ShouldNotAddSectionToShoppingList()
        {
            // Arrange
            _fixture.SetupWithExistingSection();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyAddSectionNever();
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithExistingSection_ShouldAddItemToShoppingList()
        {
            // Arrange
            _fixture.SetupWithExistingSection();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyAddItemOnce();
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithExistingSection_ShouldStoreShoppingList()
        {
            // Arrange
            _fixture.SetupWithExistingSection();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyStoringShoppingList();
        }

        #endregion WithExistingSection

        #region WithSectionIdNull

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithSectionIdNull_ShouldAddSectionToShoppingList()
        {
            // Arrange
            _fixture.SetupWithSectionIdNull();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, null, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyAddSectionOnce();
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithSectionIdNull_ShouldAddItemToShoppingList()
        {
            // Arrange
            _fixture.SetupWithSectionIdNull();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, null, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyAddItemOnce();
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithSectionIdNull_ShouldStoreShoppingList()
        {
            // Arrange
            _fixture.SetupWithSectionIdNull();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.StoreItem!,
                _fixture.ItemType.Id, null, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyStoringShoppingList();
        }

        #endregion WithSectionIdNull

        #region WithStoreMissingSection

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithStoreMissingSection_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupStoreItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupShoppingListItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupStoreNotMatchingSectionId();
            _fixture.SetupFindingStore();
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object,
                _fixture.StoreItem!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.SectionInStoreNotFound);
        }

        #endregion WithStoreMissingSection

        #region WithStoreNotFound

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithStoreNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupStoreItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupShoppingListItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupNotFindingStore();
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object,
                _fixture.StoreItem!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.StoreNotFound);
        }

        #endregion WithStoreNotFound

        #region WithItemTypeNotPartOfItem

        [Fact]
        public async Task AddItemWithTypeToShoppingList_WithItemTypeNotPartOfItem_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupStoreItem();
            _fixture.SetupSectionId();
            _fixture.SetupItemTypeNotPartOfItem();
            _fixture.SetupShoppingListMockMatchingItemType();
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object,
                _fixture.StoreItem!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemTypeNotPartOfItem);
        }

        #endregion WithItemTypeNotPartOfItem

        #endregion Item

        #region ItemId

        #region WithMissingSection

        [Fact]
        public async Task AddItemWithTypeToShoppingList_ItemId_WithMissingSection_ShouldAddItemToShoppingList()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupSectionId();
            _fixture.SetupStoreItem();
            _fixture.SetupFindingItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupShoppingListItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupStore();
            _fixture.SetupFindingStore();
            _fixture.SetupEmptyShoppingListSection();
            _fixture.SetupCreatingEmptyShoppingListSection();
            _fixture.SetupAddingSectionToShoppingList();
            _fixture.SetupAddingItemToShoppingList();
            _fixture.SetupStoringShoppingList();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id, _fixture.StoreItem.Id,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyAddItemOnce();
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_ItemId_WithMissingSection_ShouldAddSectionToShoppingList()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupSectionId();
            _fixture.SetupStoreItem();
            _fixture.SetupFindingItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupShoppingListItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupStore();
            _fixture.SetupFindingStore();
            _fixture.SetupEmptyShoppingListSection();
            _fixture.SetupCreatingEmptyShoppingListSection();
            _fixture.SetupAddingSectionToShoppingList();
            _fixture.SetupAddingItemToShoppingList();
            _fixture.SetupStoringShoppingList();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id, _fixture.StoreItem.Id,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyAddSectionOnce();
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_ItemId_WithMissingSection_ShouldStoreShoppingList()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupSectionId();
            _fixture.SetupStoreItem();
            _fixture.SetupFindingItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupShoppingListItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupStore();
            _fixture.SetupFindingStore();
            _fixture.SetupEmptyShoppingListSection();
            _fixture.SetupCreatingEmptyShoppingListSection();
            _fixture.SetupAddingSectionToShoppingList();
            _fixture.SetupAddingItemToShoppingList();
            _fixture.SetupStoringShoppingList();
            var sut = _fixture.CreateSut();

            // Act
            await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id, _fixture.StoreItem.Id,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            _fixture.VerifyStoringShoppingList();
        }

        #endregion WithMissingSection

        [Fact]
        public async Task AddItemWithTypeToShoppingList_ItemId_WithItemNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupSectionId();
            _fixture.SetupStoreItem();
            _fixture.SetupNotFindingItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupFindingShoppingList();
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id,
                _fixture.StoreItem.Id, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_ItemId_WithShoppingListNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupSectionId();
            _fixture.SetupStoreItem();
            _fixture.SetupFindingItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupNotFindingShoppingList();
            var sut = _fixture.CreateSut();

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id,
                _fixture.StoreItem.Id, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
        }

        #endregion ItemId

        private sealed class AddItemWithTypeToShoppingListFixture : LocalFixture
        {
            public IItemType ItemType { get; private set; }

            public void SetupShoppingListMockMatchingItemType()
            {
                Availability = CommonFixture.ChooseRandom(ItemType.Availabilities);

                var list = ShoppingListMother.Sections(3).WithStoreId(Availability.StoreId).Create();
                ShoppingListMock = new ShoppingListMock(list, MockBehavior.Strict);
            }

            public void SetupShoppingListItem()
            {
                ShoppingListItem = ShoppingListItemMother.InBasket().Create();
            }

            public void SetupStoreItem()
            {
                StoreItem = StoreItemMother.InitialWithTypes().Create();
            }

            public void SetupItemType()
            {
                ItemType = CommonFixture.ChooseRandom(StoreItem.ItemTypes);
            }

            public void SetupItemTypeNotPartOfItem()
            {
                ItemType = new ItemTypeBuilder().Create();
            }

            #region Mock Setup

            public void SetupCreatingShoppingListItem()
            {
                ShoppingListItemFactoryMock.SetupCreate(StoreItem.Id, ItemType.Id, false, Quantity, ShoppingListItem);
            }

            public void SetupFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListMock.Object.Id, ShoppingListMock.Object);
            }

            public void SetupNotFindingShoppingList()
            {
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListMock.Object.Id, null);
            }

            #endregion Mock Setup

            #region Aggregates

            public void SetupWithMissingSection()
            {
                SetupQuantity();
                SetupSectionId();
                SetupStoreItem();
                SetupItemType();
                SetupShoppingListMockMatchingItemType();
                SetupShoppingListItem();
                SetupCreatingShoppingListItem();
                SetupStore();
                SetupFindingStore();
                SetupEmptyShoppingListSection();
                SetupCreatingEmptyShoppingListSection();
                SetupAddingSectionToShoppingList();
                SetupAddingItemToShoppingList();
                SetupStoringShoppingList();
            }

            public void SetupWithExistingSection()
            {
                SetupQuantity();
                SetupStoreItem();
                SetupItemType();
                SetupShoppingListMockMatchingItemType();
                SetupSectionIdMatchingShoppingList();
                SetupShoppingListItem();
                SetupCreatingShoppingListItem();
                SetupStore();
                SetupFindingStore();
                SetupAddingItemToShoppingList();
                SetupStoringShoppingList();
            }

            public void SetupWithSectionIdNull()
            {
                SetupQuantity();
                SetupStoreItem();
                SetupItemType();
                SetupShoppingListMockMatchingItemType();
                SetupShoppingListItem();
                SetupCreatingShoppingListItem();
                SetupStore();
                SetupFindingStore();
                SetupEmptyShoppingListSection();
                SetupCreatingEmptyShoppingListSection();
                SetupAddingSectionToShoppingList();
                SetupAddingItemToShoppingList();
                SetupStoringShoppingList();
            }

            #endregion Aggregates
        }
    }

    private abstract class LocalFixture
    {
        protected readonly CommonFixture CommonFixture = new();
        private readonly ShoppingListSectionFactoryMock _shoppingListSectionFactoryMock;
        private readonly StoreRepositoryMock _storeRepositoryMock;
        private readonly StoreSectionFactoryMock _sectionFactoryMock;
        protected readonly ItemRepositoryMock ItemRepositoryMock;
        protected readonly ShoppingListItemFactoryMock ShoppingListItemFactoryMock;
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock;

        private IStore _store;
        private IShoppingListSection _shoppingListSection;
        protected IStoreItemAvailability Availability;

        protected LocalFixture()
        {
            _shoppingListSectionFactoryMock = new ShoppingListSectionFactoryMock(MockBehavior.Strict);
            _storeRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            ShoppingListItemFactoryMock = new ShoppingListItemFactoryMock(MockBehavior.Strict);
            ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            _sectionFactoryMock = new StoreSectionFactoryMock(MockBehavior.Strict);
        }

        public ShoppingListMock ShoppingListMock { get; protected set; }
        public SectionId? SectionId { get; protected set; }
        public QuantityInBasket Quantity { get; protected set; }
        public IStoreItem? StoreItem { get; protected set; }
        public IShoppingListItem ShoppingListItem { get; protected set; }

        public AddItemToShoppingListService CreateSut()
        {
            return new AddItemToShoppingListService(
                _shoppingListSectionFactoryMock.Object,
                _storeRepositoryMock.Object,
                ItemRepositoryMock.Object,
                ShoppingListItemFactoryMock.Object,
                ShoppingListRepositoryMock.Object);
        }

        public void SetupShoppingListMock()
        {
            ShoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(3).Create(), MockBehavior.Strict);
        }

        public void SetupQuantity()
        {
            Quantity = new QuantityInBasketBuilder().Create();
        }

        public void SetupSectionId()
        {
            SectionId = new SectionId(Guid.NewGuid());
        }

        public void SetupSectionIdMatchingShoppingList()
        {
            SectionId = CommonFixture.ChooseRandom(ShoppingListMock.Object.Sections).Id;
        }

        public void SetupStoreNotMatchingSectionId()
        {
            var sectionId = new SectionId(Guid.NewGuid());
            var section = StoreSectionMother.Default().WithId(sectionId).Create();
            var sections = new StoreSections(section.ToMonoList(), _sectionFactoryMock.Object);

            _store = StoreMother.Initial().WithId(ShoppingListMock.Object.StoreId).WithSections(sections).Create();
        }

        public void SetupStore()
        {
            var sectionId = SectionId ?? Availability.DefaultSectionId;
            var section = StoreSectionMother.Default().WithId(sectionId).Create();
            var sections = new StoreSections(section.ToMonoList(), _sectionFactoryMock.Object);

            _store = StoreMother.Initial().WithId(ShoppingListMock.Object.StoreId).WithSections(sections).Create();
        }

        public void SetupEmptyShoppingListSection()
        {
            var builder = ShoppingListSectionMother.Empty();
            if (SectionId != null)
                builder.WithId(SectionId.Value);

            _shoppingListSection = builder.Create();
        }

        #region Mock Setup

        public void SetupFindingStore()
        {
            _storeRepositoryMock.SetupFindByAsync(_store.Id, _store);
        }

        public void SetupNotFindingStore()
        {
            _storeRepositoryMock.SetupFindByAsync(ShoppingListMock.Object.StoreId, null);
        }

        public void SetupStoringShoppingList()
        {
            ShoppingListRepositoryMock.SetupStoreAsync(ShoppingListMock.Object);
        }

        public void SetupCreatingEmptyShoppingListSection()
        {
            var sectionId = SectionId ?? Availability.DefaultSectionId;
            _shoppingListSectionFactoryMock.SetupCreateEmpty(sectionId, _shoppingListSection);
        }

        public void SetupAddingSectionToShoppingList()
        {
            ShoppingListMock.SetupAddSection(_shoppingListSection);
        }

        public void SetupAddingItemToShoppingList()
        {
            var sectionId = SectionId ?? Availability.DefaultSectionId;
            ShoppingListMock.SetupAddItem(ShoppingListItem, sectionId);
        }

        public void SetupFindingItem()
        {
            ItemRepositoryMock.SetupFindByAsync(StoreItem.Id, StoreItem);
        }

        public void SetupNotFindingItem()
        {
            ItemRepositoryMock.SetupFindByAsync(StoreItem.Id, null);
        }

        #endregion Mock Setup

        #region Verify

        public void VerifyAddSectionNever()
        {
            ShoppingListMock.VerifyAddSectionNever();
        }

        public void VerifyAddSectionOnce()
        {
            ShoppingListMock.VerifyAddSectionOnce(_shoppingListSection);
        }

        public void VerifyAddItemOnce()
        {
            var sectionId = SectionId ?? Availability.DefaultSectionId;
            ShoppingListMock.VerifyAddItemOnce(ShoppingListItem, sectionId);
        }

        public void VerifyCreatingEmptyShoppingListSectionOnce()
        {
            _shoppingListSectionFactoryMock.VerifyCreateEmptyOnce(SectionId.Value);
        }

        public void VerifyStoringShoppingList()
        {
            ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
        }

        #endregion Verify
    }
}