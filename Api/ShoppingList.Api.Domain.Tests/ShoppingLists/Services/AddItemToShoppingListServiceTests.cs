using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services
{
    public class AddItemToShoppingListServiceTests
    {
        private readonly LocalFixture _local;

        public AddItemToShoppingListServiceTests()
        {
            _local = new LocalFixture();
        }

        #region ItemId

        [Fact]
        public async Task AddItemToShoppingList_WithItemIdAndShoppingListIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupStoreItem();
            local.SetupSectionId();
            local.SetupQuantity();

            // Act
            Func<Task> function = async () =>
                await service.AddItemToShoppingList(null, local.StoreItem.Id, local.SectionId, local.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupShoppingListMock();
            local.SetupStoreItemNull();
            local.SetupSectionId();
            local.SetupQuantity();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(
                local.ShoppingListMock.Object, local.StoreItem?.Id, local.SectionId, local.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupShoppingListMock();
            local.SetupStoreItem();
            local.SetupSectionId();
            local.SetupQuantity();

            local.SetupFindingNoItem();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(
                local.ShoppingListMock.Object, local.StoreItem.Id, local.SectionId, local.Quantity, default);

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
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupStoreItem();
            local.SetupShoppingListMockMatchingStoreItem();
            local.SetupShoppingListItem();

            local.SetupSectionIdMatchingShoppingList();
            local.SetupStore();
            local.SetupQuantity();

            local.SetupFindingItem();
            local.SetupCreatingShoppingListItem();
            local.SetupFindingStore();

            // Act
            await service.AddItemToShoppingList(
                local.ShoppingListMock.Object, local.StoreItem.Id, local.SectionId, local.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                local.VerifyAddSectionNever();
                local.VerifyAddItemOnce();
            }
        }

        #endregion ItemId

        #region TemporaryItemId

        [Fact]
        public async Task AddItemToShoppingList_WithTemporaryItemIdAndShoppingListIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupTemporaryStoreItem();
            local.SetupSectionId();
            local.SetupQuantity();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(null, local.StoreItem.TemporaryId,
                local.SectionId, local.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithTemporaryItemIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupShoppingListMock();
            local.SetupTemporaryStoreItemNull();
            local.SetupSectionId();
            local.SetupQuantity();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(local.ShoppingListMock.Object,
                local.StoreItem?.TemporaryId, local.SectionId, local.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithInvalidTemporaryItemId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupShoppingListMock();
            local.SetupTemporaryStoreItem();
            local.SetupSectionId();
            local.SetupQuantity();

            local.SetupFindingNoItemWithTemporaryId();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(local.ShoppingListMock.Object,
                local.StoreItem.TemporaryId, local.SectionId, local.Quantity, default);

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
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupTemporaryStoreItem();
            local.SetupShoppingListMockMatchingStoreItem();
            local.SetupShoppingListItem();

            local.SetupSectionIdMatchingShoppingList();
            local.SetupStore();
            local.SetupQuantity();

            local.SetupFindingItemWithTemporaryId();
            local.SetupCreatingShoppingListItem();
            local.SetupFindingStore();

            // Act
            await service.AddItemToShoppingList(local.ShoppingListMock.Object, local.StoreItem.TemporaryId,
                local.SectionId, local.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                local.VerifyAddSectionNever();
                local.VerifyAddItemOnce();
            }
        }

        #endregion TemporaryItemId

        #region AddItemToShoppingList (internal 1st)

        [Fact]
        public async Task AddItemToShoppingList_WithItemAtStoreNotAvailable_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupStoreItem();
            local.SetupShoppingListMockNotMatchingStoreItem();

            local.SetupSectionId();
            local.SetupQuantity();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(local.ShoppingListMock.Object,
                local.StoreItem, local.SectionId, local.Quantity, default);

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
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupStoreItem();
            local.SetupShoppingListMockMatchingStoreItem();
            local.SetupShoppingListItem();

            local.SetupSectionIdMatchingShoppingList();
            local.SetupStore();
            local.SetupQuantity();

            local.SetupCreatingShoppingListItem();
            local.SetupFindingStore();

            // Act
            await service.AddItemToShoppingList(
                local.ShoppingListMock.Object, local.StoreItem, local.SectionId, local.Quantity, default);

            // Assert
            using (new AssertionScope())
            {
                local.VerifyAddItemOnce();
            }
        }

        #endregion AddItemToShoppingList (internal 1st)

        #region AddItemToShoppingList (internal 2nd)

        [Fact]
        public async Task AddItemToShoppingList_WithStoreIdIsInvalid_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupShoppingListMock();
            local.SetupShoppingListItem();
            local.SetupSectionId();

            local.SetupFindingNoStore();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(local.ShoppingListMock.Object,
                local.ShoppingListItem, local.SectionId, default);

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
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupShoppingListMock();
            local.SetupShoppingListItem();
            local.SetupSectionId();
            local.SetupStoreNotMatchingSectionId();

            local.SetupFindingStore();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(local.ShoppingListMock.Object,
                local.ShoppingListItem, local.SectionId, default);

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
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupShoppingListMock();
            local.SetupShoppingListItem();
            local.SetupSectionId();
            local.SetupStore();
            local.SetupEmptyShoppingListSection();

            local.SetupFindingStore();
            local.SetupCreatingEmptyShoppingListSection();

            // Act
            await service.AddItemToShoppingList(local.ShoppingListMock.Object,
                local.ShoppingListItem, local.SectionId, default);

            // Assert
            using (new AssertionScope())
            {
                local.VerifyCreatingEmptyShoppingListSectionOnce();
                local.VerifyAddSectionOnce();
                local.VerifyAddItemOnce();
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithSectionAlreadyInShoppingList_ShouldNotAddSectionToShoppingList()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            local.SetupShoppingListMock();
            local.SetupShoppingListItem();
            local.SetupSectionIdMatchingShoppingList();
            local.SetupStore();

            local.SetupFindingStore();

            // Act
            await service.AddItemToShoppingList(local.ShoppingListMock.Object,
                local.ShoppingListItem, local.SectionId, default);

            // Assert
            using (new AssertionScope())
            {
                local.VerifyAddSectionNever();
                local.VerifyAddItemOnce();
            }
        }

        #endregion AddItemToShoppingList (internal 2nd)

        private class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListSectionFactoryMock ShoppingListSectionFactoryMock { get; }
            public StoreRepositoryMock StoreRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public ShoppingListItemFactoryMock ShoppingListItemFactoryMock { get; }

            public ShoppingListMock ShoppingListMock { get; private set; }
            public SectionId SectionId { get; private set; }
            public float Quantity { get; private set; }
            public IStoreItem StoreItem { get; private set; }
            public IShoppingListItem ShoppingListItem { get; private set; }
            public IStore Store { get; private set; }
            public IShoppingListSection ShoppingListSection { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListSectionFactoryMock = new ShoppingListSectionFactoryMock(Fixture);
                StoreRepositoryMock = new StoreRepositoryMock(Fixture);
                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                ShoppingListItemFactoryMock = new ShoppingListItemFactoryMock(Fixture);
            }

            public AddItemToShoppingListService CreateService()
            {
                return Fixture.Create<AddItemToShoppingListService>();
            }

            public void SetupShoppingListMock()
            {
                ShoppingListMock = new ShoppingListMock(ShoppingListMother.Sections(3).Create());
            }

            public void SetupShoppingListMockMatchingStoreItem()
            {
                var storeId = CommonFixture.ChooseRandom(StoreItem.Availabilities).StoreId;

                var list = ShoppingListMother.Sections(3).WithStoreId(storeId).Create();
                ShoppingListMock = new ShoppingListMock(list);
            }

            public void SetupShoppingListMockNotMatchingStoreItem()
            {
                var excludedStoreIds = StoreItem.Availabilities.Select(av => av.StoreId.Value);
                var storeId = new StoreId(CommonFixture.NextInt(excludedStoreIds));

                var list = ShoppingListMother.Sections(3).WithStoreId(storeId).Create();
                ShoppingListMock = new ShoppingListMock(list);
            }

            public void SetupShoppingListItem()
            {
                ShoppingListItem = ShoppingListItemMother.InBasket().Create();
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

            public void SetupSectionId()
            {
                SectionId = new SectionId(CommonFixture.NextInt());
            }

            public void SetupSectionIdMatchingShoppingList()
            {
                SectionId = CommonFixture.ChooseRandom(ShoppingListMock.Object.Sections).Id;
            }

            public void SetupQuantity()
            {
                Quantity = CommonFixture.NextFloat();
            }

            public void SetupStore()
            {
                var section = StoreSectionMother.Default().WithId(SectionId).Create();

                Store = StoreMother.Initial().WithId(ShoppingListMock.Object.StoreId).WithSection(section).Create();
            }

            public void SetupStoreNotMatchingSectionId()
            {
                var sectionId = new SectionId(CommonFixture.NextInt(SectionId.Value));
                var section = StoreSectionMother.Default().WithId(sectionId).Create();

                Store = StoreMother.Initial().WithId(ShoppingListMock.Object.StoreId).WithSection(section).Create();
            }

            public void SetupEmptyShoppingListSection()
            {
                ShoppingListSection = ShoppingListSectionMother.Empty().WithId(SectionId).Create();
            }

            #region Mock Setup

            public void SetupFindingNoItem()
            {
                ItemRepositoryMock.SetupFindByAsync(StoreItem.Id, null);
            }

            public void SetupFindingNoItemWithTemporaryId()
            {
                ItemRepositoryMock.SetupFindByAsync(StoreItem.TemporaryId, null);
            }

            public void SetupFindingItem()
            {
                ItemRepositoryMock.SetupFindByAsync(StoreItem.Id, StoreItem);
            }

            public void SetupFindingItemWithTemporaryId()
            {
                ItemRepositoryMock.SetupFindByAsync(StoreItem.TemporaryId, StoreItem);
            }

            public void SetupCreatingShoppingListItem()
            {
                ShoppingListItemFactoryMock.SetupCreate(StoreItem.Id, false, Quantity, ShoppingListItem);
            }

            public void SetupFindingStore()
            {
                StoreRepositoryMock.SetupFindByAsync(Store.Id, Store);
            }

            public void SetupFindingNoStore()
            {
                StoreRepositoryMock.SetupFindByAsync(ShoppingListMock.Object.StoreId, null);
            }

            public void SetupCreatingEmptyShoppingListSection()
            {
                ShoppingListSectionFactoryMock.SetupCreateEmpty(SectionId, ShoppingListSection);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyAddSectionNever()
            {
                ShoppingListMock.VerifyAddSectionNever();
            }

            public void VerifyAddSectionOnce()
            {
                ShoppingListMock.VerifyAddSectionOnce(ShoppingListSection);
            }

            public void VerifyAddItemOnce()
            {
                ShoppingListMock.VerifyAddItemOnce(ShoppingListItem, SectionId);
            }

            public void VerifyCreatingEmptyShoppingListSectionOnce()
            {
                ShoppingListSectionFactoryMock.VerifyCreateEmptyOnce(SectionId);
            }

            #endregion Verify
        }
    }
}