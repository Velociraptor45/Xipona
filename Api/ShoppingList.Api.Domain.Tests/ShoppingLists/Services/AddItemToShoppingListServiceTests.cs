using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services.AddItems;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services;

public class AddItemToShoppingListServiceTests
{
    public class AddItemAsyncTests
    {
        private readonly AddItemAsyncFixture _fixture;

        public AddItemAsyncTests()
        {
            _fixture = new AddItemAsyncFixture();
        }

        #region ItemId

        [Fact]
        public async Task AddItemAsync_WithInvalidItemId_ShouldThrowDomainException()
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
            Func<Task> function = async () => await service.AddItemAsync(
                _fixture.ShoppingListMock.Object, _fixture.Item.Id, _fixture.SectionId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task AddItemAsync_WithItemIdAndValidData_ShouldAddItemToShoppingList()
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
            await service.AddItemAsync(
                _fixture.ShoppingListMock.Object, _fixture.Item.Id, _fixture.SectionId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyAddSectionNever();
                _fixture.VerifyAddItemOnce();
            }
        }

        #endregion ItemId

        #region AddItemAsync (internal 1st)

        [Fact]
        public async Task AddItemAsync_WithItemAtStoreNotAvailable_ShouldThrowDomainException()
        {
            // Arrange
            var service = _fixture.CreateSut();

            _fixture.SetupItem();
            _fixture.SetupShoppingListMockNotMatchingItem();

            _fixture.SetupSectionId();
            _fixture.SetupQuantity();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ShoppingListMock);

            // Act
            Func<Task> function = async () => await service.AddItemAsync(_fixture.ShoppingListMock.Object,
                _fixture.Item!, null, _fixture.SectionId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemAtStoreNotAvailable);
            }
        }

        [Fact]
        public async Task AddItemAsync_WithSectionIdIsNull_ShouldSetDefaultSectionId()
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
            await service.AddItemAsync(
                _fixture.ShoppingListMock.Object, _fixture.Item!, null, _fixture.SectionId, _fixture.Quantity);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyAddItemOnce();
            }
        }

        #endregion AddItemAsync (internal 1st)

        #region AddItemAsync (internal 2nd)

        [Fact]
        public async Task AddItemAsync_WithStoreIdIsInvalid_ShouldThrowDomainException()
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
            Func<Task> function = async () => await service.AddItemAsync(_fixture.ShoppingListMock.Object,
                _fixture.ShoppingListItem, null, _fixture.SectionId.Value);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task AddItemAsync_WithSectionIdNotInStore_ShouldThrowDomainException()
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
            Func<Task> function = async () => await service.AddItemAsync(_fixture.ShoppingListMock.Object,
                _fixture.ShoppingListItem, null, _fixture.SectionId.Value);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.SectionInStoreNotFound);
            }
        }

        [Fact]
        public async Task AddItemAsync_WithSectionNotInShoppingList_AddSectionToShoppingList()
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
            await service.AddItemAsync(_fixture.ShoppingListMock.Object,
                _fixture.ShoppingListItem, null, _fixture.SectionId.Value);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyCreatingEmptyShoppingListSectionOnce();
                _fixture.VerifyAddSectionOnce();
                _fixture.VerifyAddItemOnce();
            }
        }

        [Fact]
        public async Task AddItemAsync_WithSectionAlreadyInShoppingList_ShouldNotAddSectionToShoppingList()
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
            await service.AddItemAsync(_fixture.ShoppingListMock.Object,
                _fixture.ShoppingListItem, null, _fixture.SectionId.Value);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyAddSectionNever();
                _fixture.VerifyAddItemOnce();
            }
        }

        #endregion AddItemAsync (internal 2nd)

        private sealed class AddItemAsyncFixture : AddItemToShoppingListServiceFixture
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

            #region Mock Setup

            public void SetupCreatingShoppingListItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Item);
                TestPropertyNotSetException.ThrowIfNull(ShoppingListItem);
                ShoppingListItemFactoryMock.SetupCreate(Item.Id, null, false, Quantity, ShoppingListItem);
            }

            #endregion Mock Setup
        }
    }

    public class AddItemWithTypeAsyncTests
    {
        private readonly AddItemWithTypeAsyncFixture _fixture;

        public AddItemWithTypeAsyncTests()
        {
            _fixture = new AddItemWithTypeAsyncFixture();
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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, null, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, null, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object, _fixture.Item!,
                _fixture.ItemType.Id, null, _fixture.Quantity);

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
            Func<Task> func = async () => await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object,
                _fixture.Item!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            Func<Task> func = async () => await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object,
                _fixture.Item!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            Func<Task> func = async () => await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object,
                _fixture.Item!, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object.Id, _fixture.Item.Id,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object.Id, _fixture.Item.Id,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

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
            await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object.Id, _fixture.Item.Id,
                _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

            // Assert
            _fixture.VerifyStoringShoppingList();
        }

        #endregion WithMissingSection

        [Fact]
        public async Task AddItemWithTypeAsync_ItemId_WithItemNotFound_ShouldThrowDomainException()
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
            Func<Task> func = async () => await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object.Id,
                _fixture.Item.Id, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }

        [Fact]
        public async Task AddItemWithTypeAsync_ItemId_WithShoppingListNotFound_ShouldThrowDomainException()
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
            Func<Task> func = async () => await sut.AddItemWithTypeAsync(_fixture.ShoppingListMock.Object.Id,
                _fixture.Item.Id, _fixture.ItemType.Id, _fixture.SectionId, _fixture.Quantity);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ShoppingListNotFound);
        }

        #endregion ItemId

        private sealed class AddItemWithTypeAsyncFixture : AddItemToShoppingListServiceFixture
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

    public class AddAsync
    {
        private readonly AddAsyncFixture _fixture = new();

        [Fact]
        public async Task AddAsync_WithTwoItems_WithOneAlreadyExisting_ShouldAddBothItems()
        {
            // Arrange
            // first item: without types & already existing
            // second item: with types & not existing
            _fixture.SetupItemsToAdd();
            _fixture.SetupShoppingListsWithOneItemAlreadyExisting();
            _fixture.SetupFindingShoppingLists();
            _fixture.SetupFindingStores();
            _fixture.SetupFindingItems();
            _fixture.SetupExpectedShoppingListItems();
            _fixture.SetupCreatingNewSection();
            _fixture.SetupStoringShoppingLists();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemsToAdd);

            // Act
            await sut.AddAsync(_fixture.ItemsToAdd);

            // Assert
            _fixture.VerifyStoringShoppingLists();
            _fixture.VerifyAddingFirstItem();
            _fixture.VerifyAddingSecondItem();
        }

        private sealed class AddAsyncFixture : AddItemToShoppingListServiceFixture
        {
            private IReadOnlyCollection<IShoppingList>? _shoppingLists;
            private ShoppingListItem? _firstExpectedShoppingListItem;
            private ShoppingListItem? _secondExpectedShoppingListItem;
            private IShoppingListItem? _alreadyExistingItem;
            private IReadOnlyCollection<IStore>? _stores;
            private IReadOnlyCollection<IItem>? _items;

            private IEnumerable<StoreId> StoreIds
            {
                get
                {
                    TestPropertyNotSetException.ThrowIfNull(ItemsToAdd);

                    return ItemsToAdd.Select(i => i.StoreId).Distinct();
                }
            }

            private IEnumerable<ItemId> ItemIds
            {
                get
                {
                    TestPropertyNotSetException.ThrowIfNull(ItemsToAdd);

                    return ItemsToAdd.Select(i => i.ItemId).Distinct();
                }
            }

            public IReadOnlyCollection<ItemToShoppingListAddition>? ItemsToAdd { get; private set; }

            public void SetupItemsToAdd()
            {
                ItemsToAdd = new List<ItemToShoppingListAddition>
                {
                    new ItemToShoppingListAdditionBuilder().WithoutItemTypeId().Create(),
                    new ItemToShoppingListAdditionBuilder().Create(),
                };
            }

            public void SetupFindingStores()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemsToAdd);
                TestPropertyNotSetException.ThrowIfNull(_shoppingLists);

                var storeIds = StoreIds.ToList();
                var firstExistingSectionId = _shoppingLists.First().Sections.First().Id;
                var firstExistingSection = SectionMother.Default().WithId(firstExistingSectionId).Create();
                _stores = new List<IStore>
                {
                    StoreMother.Section(firstExistingSection).WithId(storeIds[0]).Create(),
                    StoreMother.Initial().WithId(storeIds[1]).Create(),
                };
                StoreRepositoryMock.SetupFindActiveByAsync(storeIds, _stores);
            }

            public void SetupFindingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemsToAdd);
                TestPropertyNotSetException.ThrowIfNull(_stores);

                var firstItem = ItemsToAdd.First();
                var secondItem = ItemsToAdd.Last();

                var firstItemAvailability = ItemAvailabilityMother
                    .ForStore(firstItem.StoreId)
                    .WithDefaultSectionId(_stores.First().Sections.First().Id)
                    .Create();
                var secondItemAvailability = ItemAvailabilityMother
                    .ForStore(secondItem.StoreId)
                    .WithDefaultSectionId(_stores.Last().Sections.First().Id)
                    .Create();

                var itemType = ItemTypeMother.InitialAvailableAt(secondItemAvailability)
                    .WithId(secondItem.ItemTypeId!.Value)
                    .Create();

                _items = new List<IItem>
                {
                    ItemMother.Initial().WithId(firstItem.ItemId).WithAvailability(firstItemAvailability).Create(),
                    ItemMother.InitialWithType(itemType).WithId(secondItem.ItemId).Create(),
                };

                ItemRepositoryMock.SetupFindActiveByAsync(ItemIds.ToList(), _items);
            }

            public void SetupShoppingListsWithOneItemAlreadyExisting()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemsToAdd);

                var storeIds = StoreIds.ToList();

                var firstListSection = ShoppingListSectionMother.Item(ItemsToAdd.First().ItemId, null).CreateMany(1).ToList();
                _alreadyExistingItem = firstListSection.First().Items.Single();
                _shoppingLists = new List<IShoppingList>
                {
                    ShoppingListMother.Initial().WithSections(firstListSection).WithStoreId(storeIds[0]).Create(),
                    ShoppingListMother.Initial().WithStoreId(storeIds[1]).Create(),
                };
            }

            public void SetupFindingShoppingLists()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingLists);

                ShoppingListRepositoryMock.SetupFindActiveByAsync(StoreIds, _shoppingLists);
            }

            public void SetupStoringShoppingLists()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingLists);
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingLists.First());
                ShoppingListRepositoryMock.SetupStoreAsync(_shoppingLists.Last());
            }

            public void SetupExpectedShoppingListItems()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemsToAdd);
                TestPropertyNotSetException.ThrowIfNull(_shoppingLists);
                TestPropertyNotSetException.ThrowIfNull(_alreadyExistingItem);

                var firstItem = ItemsToAdd.First();
                var secondItem = ItemsToAdd.Last();

                _firstExpectedShoppingListItem = new ShoppingListItem(
                    _alreadyExistingItem.Id, _alreadyExistingItem.TypeId, _alreadyExistingItem.IsInBasket,
                    _alreadyExistingItem.Quantity + firstItem.Quantity);
                var firstCreatedShoppingListItem = new ShoppingListItem(
                    _alreadyExistingItem.Id, _alreadyExistingItem.TypeId, _alreadyExistingItem.IsInBasket,
                    firstItem.Quantity);
                ShoppingListItemFactoryMock.SetupCreate(firstItem.ItemId, null, false,
                    firstItem.Quantity, firstCreatedShoppingListItem);

                _secondExpectedShoppingListItem = ShoppingListItemMother.NotInBasket().Create();
                ShoppingListItemFactoryMock.SetupCreate(secondItem.ItemId, secondItem.ItemTypeId, false,
                    secondItem.Quantity, _secondExpectedShoppingListItem);
            }

            public void SetupCreatingNewSection()
            {
                TestPropertyNotSetException.ThrowIfNull(_items);

                var sectionId = _items.Last().ItemTypes.First().Availabilities.First().DefaultSectionId;
                var section = new ShoppingListSectionBuilder().WithId(sectionId).Create();
                ShoppingListSectionFactoryMock.SetupCreateEmpty(sectionId, section);
            }

            public void VerifyAddingFirstItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingLists);
                TestPropertyNotSetException.ThrowIfNull(_firstExpectedShoppingListItem);
                _shoppingLists.First().Items.Should().Contain(i => i.IsEquivalentTo(_firstExpectedShoppingListItem));
            }

            public void VerifyAddingSecondItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingLists);
                TestPropertyNotSetException.ThrowIfNull(_secondExpectedShoppingListItem);

                _shoppingLists.Last().Items.Should().Contain(i => i.IsEquivalentTo(_secondExpectedShoppingListItem));
            }

            public void VerifyStoringShoppingLists()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingLists);
                ShoppingListRepositoryMock.VerifyStoreAsync(_shoppingLists.First(), Times.Once);
                ShoppingListRepositoryMock.VerifyStoreAsync(_shoppingLists.Last(), Times.Once);
            }
        }
    }

    private abstract class AddItemToShoppingListServiceFixture
    {
        protected readonly CommonFixture CommonFixture = new();
        protected readonly ShoppingListSectionFactoryMock ShoppingListSectionFactoryMock = new(MockBehavior.Strict);
        protected readonly StoreRepositoryMock StoreRepositoryMock = new(MockBehavior.Strict);
        private readonly SectionFactoryMock _sectionFactoryMock = new(MockBehavior.Strict);
        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected readonly ShoppingListItemFactoryMock ShoppingListItemFactoryMock = new(MockBehavior.Strict);
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock = new(MockBehavior.Strict);

        private IStore? _store;
        private IShoppingListSection? _shoppingListSection;
        protected IItemAvailability? Availability;

        public ShoppingListMock? ShoppingListMock { get; protected set; }
        public SectionId? SectionId { get; private set; }
        public QuantityInBasket Quantity { get; private set; }
        public IItem? Item { get; protected set; }
        public IShoppingListItem? ShoppingListItem { get; protected set; }

        public AddItemToShoppingListService CreateSut()
        {
            return new AddItemToShoppingListService(
                ShoppingListSectionFactoryMock.Object,
                _ => StoreRepositoryMock.Object,
                ItemRepositoryMock.Object,
                ShoppingListItemFactoryMock.Object,
                ShoppingListRepositoryMock.Object,
                default);
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
            StoreRepositoryMock.SetupFindActiveByAsync(_store.Id, _store);
        }

        public void SetupNotFindingStore()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            StoreRepositoryMock.SetupFindActiveByAsync(ShoppingListMock.Object.StoreId, null);
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
            ShoppingListSectionFactoryMock.SetupCreateEmpty(sectionId, _shoppingListSection);
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
            ItemRepositoryMock.SetupFindActiveByAsync(Item.Id, Item);
        }

        public void SetupNotFindingItem()
        {
            TestPropertyNotSetException.ThrowIfNull(Item);
            ItemRepositoryMock.SetupFindActiveByAsync(Item.Id, null);
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
            ShoppingListSectionFactoryMock.VerifyCreateEmptyOnce(SectionId.Value);
        }

        public void VerifyStoringShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(ShoppingListMock);
            ShoppingListRepositoryMock.VerifyStoreAsyncOnce(ShoppingListMock.Object);
        }

        #endregion Verify
    }
}