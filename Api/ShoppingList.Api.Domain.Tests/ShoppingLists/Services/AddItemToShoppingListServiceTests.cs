using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Items.Models;
using ShoppingList.Api.Domain.TestKit.Items.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ShoppingList.Api.TestTools.Exceptions;

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
            _fixture.SetupItem();
            _fixture.SetupSectionId();
            _fixture.SetupQuantity();

            _fixture.SetupNotFindingItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingListAsync(
                _fixture.ShoppingListMock.Object, _fixture.Item.Id, _fixture.SectionId, _fixture.Quantity, default);

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

            _fixture.SetupItem();
            _fixture.SetupShoppingListMockMatchingItem();
            _fixture.SetupShoppingListItem();

            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupStore();
            _fixture.SetupQuantity();

            _fixture.SetupFindingItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupFindingStore();

            _fixture.SetupAddingItemToShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);

            // Act
            await service.AddItemToShoppingListAsync(
                _fixture.ShoppingListMock.Object, _fixture.Item.Id, _fixture.SectionId, _fixture.Quantity, default);

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
            _fixture.SetupTemporaryItem();
            _fixture.SetupSectionId();
            _fixture.SetupQuantity();

            _fixture.SetupFindingNoItemWithTemporaryId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item.TemporaryId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.Item.TemporaryId.Value, _fixture.SectionId, _fixture.Quantity, default);

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

            _fixture.SetupTemporaryItem();
            _fixture.SetupShoppingListMockMatchingItem();
            _fixture.SetupShoppingListItem();

            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupStore();
            _fixture.SetupQuantity();

            _fixture.SetupFindingItemWithTemporaryId();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupFindingStore();
            _fixture.SetupAddingItemToShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Item.TemporaryId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);

            // Act
            await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.Item.TemporaryId.Value, _fixture.SectionId, _fixture.Quantity, default);

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

            _fixture.SetupItem();
            _fixture.SetupShoppingListMockNotMatchingItem();

            _fixture.SetupSectionId();
            _fixture.SetupQuantity();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingListAsync(_fixture.ShoppingListMock.Object,
                _fixture.Item!, _fixture.SectionId, _fixture.Quantity, default);

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

            _fixture.SetupItem();
            _fixture.SetupShoppingListMockMatchingItem();
            _fixture.SetupShoppingListItem();

            _fixture.SetupStore();
            _fixture.SetupQuantity();

            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupFindingStore();

            _fixture.SetupEmptyShoppingListSection();
            _fixture.SetupCreatingEmptyShoppingListSection();
            _fixture.SetupAddingSectionToShoppingList();
            _fixture.SetupAddingItemToShoppingList();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);

            // Act
            await service.AddItemToShoppingListAsync(
                _fixture.ShoppingListMock.Object, _fixture.Item!, _fixture.SectionId, _fixture.Quantity, default);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListItem);

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

            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListItem);

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
            public void SetupShoppingListMockMatchingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                Availability = CommonFixture.ChooseRandom(Item.Availabilities);

                var list = ShoppingListMother.Sections(3).WithStoreId(Availability.StoreId).Create();
                ShoppingListMock = new ShoppingListMock(list, MockBehavior.Strict);
            }

            public void SetupShoppingListMockNotMatchingItem()
            {
                var storeId = new StoreIdBuilder().Create();

                var list = ShoppingListMother.Sections(3).WithStoreId(storeId).Create();
                ShoppingListMock = new ShoppingListMock(list, MockBehavior.Strict);
            }

            public void SetupShoppingListItem()
            {
                ShoppingListItem = ShoppingListItemMother.InBasket().WithoutTypeId().Create();
            }

            public void SetupItem()
            {
                Item = ItemMother.Initial().Create();
            }

            public void SetupTemporaryItem()
            {
                Item = ItemMother.InitialTemporary().Create();
            }

            #region Mock Setup

            public void SetupCreatingShoppingListItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                TestPropertyNotSetException.ThrowIfNull(ShoppingListItem);
                ShoppingListItemFactoryMock.SetupCreate(Item.Id, null, false, Quantity, ShoppingListItem);
            }

            public void SetupFindingNoItemWithTemporaryId()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                TestPropertyNotSetException.ThrowIfNull(Item.TemporaryId);
                ItemRepositoryMock.SetupFindByAsync(Item.TemporaryId.Value, null);
            }

            public void SetupFindingItemWithTemporaryId()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                TestPropertyNotSetException.ThrowIfNull(Item.TemporaryId);
                ItemRepositoryMock.SetupFindByAsync(Item.TemporaryId.Value, Item);
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object, _fixture.Item!,
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
            _fixture.SetupItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupShoppingListItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupStoreNotMatchingSectionId();
            _fixture.SetupFindingStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object,
                _fixture.Item!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

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
            _fixture.SetupItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupSectionIdMatchingShoppingList();
            _fixture.SetupShoppingListItem();
            _fixture.SetupCreatingShoppingListItem();
            _fixture.SetupNotFindingStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object,
                _fixture.Item!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

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
            _fixture.SetupItem();
            _fixture.SetupSectionId();
            _fixture.SetupItemTypeNotPartOfItem();
            _fixture.SetupShoppingListMockMatchingItemType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingList(_fixture.ShoppingListMock.Object,
                _fixture.Item!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

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
            _fixture.SetupItem();
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id, _fixture.Item.Id,
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
            _fixture.SetupItem();
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id, _fixture.Item.Id,
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
            _fixture.SetupItem();
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id, _fixture.Item.Id,
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
            _fixture.SetupItem();
            _fixture.SetupNotFindingItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupFindingShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id,
                _fixture.Item.Id, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }

        [Fact]
        public async Task AddItemWithTypeToShoppingList_ItemId_WithShoppingListNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupQuantity();
            _fixture.SetupSectionId();
            _fixture.SetupItem();
            _fixture.SetupFindingItem();
            _fixture.SetupItemType();
            _fixture.SetupShoppingListMockMatchingItemType();
            _fixture.SetupNotFindingShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemType);

            // Act
            Func<Task> func = async () => await sut.AddItemWithTypeToShoppingListAsync(_fixture.ShoppingListMock.Object.Id,
                _fixture.Item.Id, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity, default);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
        }

        #endregion ItemId

        private sealed class AddItemWithTypeToShoppingListFixture : LocalFixture
        {
            public IItemType? ItemType { get; private set; }

            public void SetupShoppingListMockMatchingItemType()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemType);
                Availability = CommonFixture.ChooseRandom(ItemType.Availabilities);

                var list = ShoppingListMother.Sections(3).WithStoreId(Availability.StoreId).Create();
                ShoppingListMock = new ShoppingListMock(list, MockBehavior.Strict);
            }

            public void SetupShoppingListItem()
            {
                ShoppingListItem = ShoppingListItemMother.InBasket().Create();
            }

            public void SetupItem()
            {
                Item = ItemMother.InitialWithTypes().Create();
            }

            public void SetupItemType()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                ItemType = CommonFixture.ChooseRandom(Item.ItemTypes);
            }

            public void SetupItemTypeNotPartOfItem()
            {
                ItemType = new ItemTypeBuilder().Create();
            }

            #region Mock Setup

            public void SetupCreatingShoppingListItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                TestPropertyNotSetException.ThrowIfNull(ItemType);
                TestPropertyNotSetException.ThrowIfNull(ShoppingListItem);
                ShoppingListItemFactoryMock.SetupCreate(Item.Id, ItemType.Id, false, Quantity, ShoppingListItem);
            }

            public void SetupFindingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListMock.Object.Id, ShoppingListMock.Object);
            }

            public void SetupNotFindingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
                ShoppingListRepositoryMock.SetupFindByAsync(ShoppingListMock.Object.Id, null);
            }

            #endregion Mock Setup

            #region Aggregates

            public void SetupWithMissingSection()
            {
                SetupQuantity();
                SetupSectionId();
                SetupItem();
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
                SetupItem();
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
                SetupItem();
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
        private readonly SectionFactoryMock _sectionFactoryMock;
        protected readonly ItemRepositoryMock ItemRepositoryMock;
        protected readonly ShoppingListItemFactoryMock ShoppingListItemFactoryMock;
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock;

        private IStore? _store;
        private IShoppingListSection? _shoppingListSection;
        protected IItemAvailability? Availability;

        protected LocalFixture()
        {
            _shoppingListSectionFactoryMock = new ShoppingListSectionFactoryMock(MockBehavior.Strict);
            _storeRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            ShoppingListItemFactoryMock = new ShoppingListItemFactoryMock(MockBehavior.Strict);
            ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
            _sectionFactoryMock = new SectionFactoryMock(MockBehavior.Strict);
        }

        public ShoppingListMock? ShoppingListMock { get; protected set; }
        public SectionId? SectionId { get; private set; }
        public QuantityInBasket Quantity { get; private set; }
        public IItem? Item { get; protected set; }
        public IShoppingListItem? ShoppingListItem { get; protected set; }

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
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            SectionId = CommonFixture.ChooseRandom(ShoppingListMock.Object.Sections).Id;
        }

        public void SetupStoreNotMatchingSectionId()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            var sectionId = new SectionId(Guid.NewGuid());
            var section = SectionMother.Default().WithId(sectionId).Create();
            var sections = new Sections(section.ToMonoList(), _sectionFactoryMock.Object);

            _store = StoreMother.Initial().WithId(ShoppingListMock.Object.StoreId).WithSections(sections).Create();
        }

        public void SetupStore()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            var sectionId = SectionId
                            ?? Availability?.DefaultSectionId
                            ?? throw new TestPropertyNotSetException(nameof(Availability));
            var section = SectionMother.Default().WithId(sectionId).Create();
            var sections = new Sections(section.ToMonoList(), _sectionFactoryMock.Object);

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
            TestPropertyNotSetException.ThrowIfNull(_store);
            _storeRepositoryMock.SetupFindByAsync(_store.Id, _store);
        }

        public void SetupNotFindingStore()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            _storeRepositoryMock.SetupFindByAsync(ShoppingListMock.Object.StoreId, null);
        }

        public void SetupStoringShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            ShoppingListRepositoryMock.SetupStoreAsync(ShoppingListMock.Object);
        }

        public void SetupCreatingEmptyShoppingListSection()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListSection);
            var sectionId = SectionId
                            ?? Availability?.DefaultSectionId
                            ?? throw new TestPropertyNotSetException(nameof(Availability));
            _shoppingListSectionFactoryMock.SetupCreateEmpty(sectionId, _shoppingListSection);
        }

        public void SetupAddingSectionToShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListSection);
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            ShoppingListMock.SetupAddSection(_shoppingListSection);
        }

        public void SetupAddingItemToShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(ShoppingListItem);
            var sectionId = SectionId
                            ?? Availability?.DefaultSectionId
                            ?? throw new TestPropertyNotSetException(nameof(Availability));
            ShoppingListMock.SetupAddItem(ShoppingListItem, sectionId);
        }

        public void SetupFindingItem()
        {
            TestPropertyNotSetException.ThrowIfNull(Item);
            ItemRepositoryMock.SetupFindByAsync(Item.Id, Item);
        }

        public void SetupNotFindingItem()
        {
            TestPropertyNotSetException.ThrowIfNull(Item);
            ItemRepositoryMock.SetupFindByAsync(Item.Id, null);
        }

        #endregion Mock Setup

        #region Verify

        public void VerifyAddSectionNever()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            ShoppingListMock.VerifyAddSectionNever();
        }

        public void VerifyAddSectionOnce()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(_shoppingListSection);
            ShoppingListMock.VerifyAddSectionOnce(_shoppingListSection);
        }

        public void VerifyAddItemOnce()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            TestPropertyNotSetException.ThrowIfNull(ShoppingListItem);
            var sectionId = SectionId
                            ?? Availability?.DefaultSectionId
                            ?? throw new TestPropertyNotSetException(nameof(Availability));
            ShoppingListMock.VerifyAddItemOnce(ShoppingListItem, sectionId);
        }

        public void VerifyCreatingEmptyShoppingListSectionOnce()
        {
            TestPropertyNotSetException.ThrowIfNull(SectionId);
            _shoppingListSectionFactoryMock.VerifyCreateEmptyOnce(SectionId.Value);
        }

        public void VerifyStoringShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
        }

        #endregion Verify
    }
}