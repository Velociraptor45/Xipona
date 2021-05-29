using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Ports;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Services
{
    public class AddItemToShoppingListServiceTests
    {
        #region ItemId

        [Fact]
        public async Task AddItemToShoppingList_WithItemIdAndShoppingListIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var itemId = new ItemId(local.CommonFixture.NextInt());
            var sectionId = new SectionId(local.CommonFixture.NextInt());
            var quantity = local.CommonFixture.NextFloat();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(null, itemId, sectionId, quantity, default);

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

            var shoppingList = local.ShoppingListFixture.AsModelFixture().CreateValid();
            var sectionId = new SectionId(local.CommonFixture.NextInt());
            var quantity = local.CommonFixture.NextFloat();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(shoppingList, (ItemId)null,
                sectionId, quantity, default);

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

            var shoppingList = local.ShoppingListFixture.AsModelFixture().CreateValid();
            var itemId = new ItemId(local.CommonFixture.NextInt());
            var sectionId = new SectionId(local.CommonFixture.NextInt());
            var quantity = local.CommonFixture.NextFloat();

            local.ItemRepositoryMock.SetupFindByAsync(itemId, null);

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(shoppingList, itemId,
                sectionId, quantity, default);

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

            var storeItem = local.StoreItemFixture.CreateValid();
            var shoppingListMock = local.CreateValidShoppingListMock(storeItem);
            var shoppingListItem = local.ShoppingListItemFixture.AsModelFixture().CreateValid();

            SectionId sectionId = local.CommonFixture.ChooseRandom(shoppingListMock.Object.Sections).Id;
            var store = local.CreateStore(shoppingListMock.Object, sectionId);

            float quantity = local.CommonFixture.NextFloat();

            local.ItemRepositoryMock.SetupFindByAsync(storeItem.Id, storeItem);
            local.ShoppingListItemFactoryMock.SetupCreate(storeItem.Id, false, quantity, shoppingListItem);
            local.StoreRepositoryMock.SetupFindByAsync(store.Id, store);

            // Act
            await service.AddItemToShoppingList(shoppingListMock.Object, storeItem.Id, sectionId, quantity, default);

            // Assert
            using (new AssertionScope())
            {
                shoppingListMock.VerifyAddSectionNever();
                shoppingListMock.VerifyAddItemOnce(shoppingListItem, sectionId);
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

            var temporaryItemId = new TemporaryItemId(Guid.NewGuid());
            var sectionId = new SectionId(local.CommonFixture.NextInt());
            var quantity = local.CommonFixture.NextFloat();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(null, temporaryItemId, sectionId,
                quantity, default);

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

            var shoppingList = local.ShoppingListFixture.AsModelFixture().CreateValid();
            var sectionId = new SectionId(local.CommonFixture.NextInt());
            var quantity = local.CommonFixture.NextFloat();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(shoppingList,
                (TemporaryItemId)null, sectionId, quantity, default);

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

            var shoppingList = local.ShoppingListFixture.AsModelFixture().CreateValid();
            var temporaryItemId = new TemporaryItemId(Guid.NewGuid());
            var sectionId = new SectionId(local.CommonFixture.NextInt());
            var quantity = local.CommonFixture.NextFloat();

            local.ItemRepositoryMock.SetupFindByAsync(temporaryItemId, null);

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(shoppingList, temporaryItemId,
                sectionId, quantity, default);

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

            var storeItem = local.StoreItemFixture.CreateValid();
            var shoppingListMock = local.CreateValidShoppingListMock(storeItem);
            var shoppingListItem = local.ShoppingListItemFixture.AsModelFixture().CreateValid();

            SectionId sectionId = local.CommonFixture.ChooseRandom(shoppingListMock.Object.Sections).Id;
            var store = local.CreateStore(shoppingListMock.Object, sectionId);

            float quantity = local.CommonFixture.NextFloat();

            local.ItemRepositoryMock.SetupFindByAsync(storeItem.TemporaryId, storeItem);
            local.ShoppingListItemFactoryMock.SetupCreate(storeItem.Id, false, quantity, shoppingListItem);
            local.StoreRepositoryMock.SetupFindByAsync(store.Id, store);

            // Act
            await service.AddItemToShoppingList(shoppingListMock.Object, storeItem.TemporaryId, sectionId, quantity, default);

            // Assert
            using (new AssertionScope())
            {
                shoppingListMock.VerifyAddSectionNever();
                shoppingListMock.VerifyAddItemOnce(shoppingListItem, sectionId);
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

            var storeItem = local.StoreItemFixture.CreateValid();
            var shoppingListMock = local.CreateShoppingListMockWithIncompatibleStore(storeItem);

            var sectionId = new SectionId(local.CommonFixture.NextInt());
            var quantity = local.CommonFixture.NextFloat();

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(shoppingListMock.Object, storeItem,
                sectionId, quantity, default);

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

            var storeItem = local.StoreItemFixture.CreateValid();
            var shoppingListMock = local.CreateValidShoppingListMock(storeItem);
            var shoppingListItem = local.ShoppingListItemFixture.AsModelFixture().CreateValid();

            SectionId sectionId = storeItem.GetDefaultSectionIdForStore(shoppingListMock.Object.StoreId);
            var store = local.CreateStore(shoppingListMock.Object, sectionId);

            float quantity = local.CommonFixture.NextFloat();

            local.ShoppingListItemFactoryMock.SetupCreate(storeItem.Id, false, quantity, shoppingListItem);
            local.StoreRepositoryMock.SetupFindByAsync(store.Id, store);

            // Act
            await service.AddItemToShoppingList(shoppingListMock.Object, storeItem, sectionId: null, quantity, default);

            // Assert
            using (new AssertionScope())
            {
                shoppingListMock.VerifyAddItemOnce(shoppingListItem, sectionId);
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

            var shoppingList = local.ShoppingListFixture.AsModelFixture().CreateValid();
            var shoppingListItem = local.ShoppingListItemFixture.AsModelFixture().CreateValid();

            SectionId sectionId = local.CommonFixture.ChooseRandom(shoppingList.Sections).Id;

            local.StoreRepositoryMock.SetupFindByAsync(shoppingList.StoreId, null);

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(shoppingList, shoppingListItem,
                sectionId, default);

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

            var shoppingList = local.ShoppingListFixture.AsModelFixture().CreateValid();
            var shoppingListItem = local.ShoppingListItemFixture.AsModelFixture().CreateValid();

            var store = local.StoreFixture.CreateValid();
            SectionId sectionId = local.CreateSectionIdNotInStore(store);

            local.StoreRepositoryMock.SetupFindByAsync(shoppingList.StoreId, store);

            // Act
            Func<Task> function = async () => await service.AddItemToShoppingList(shoppingList, shoppingListItem,
                sectionId, default);

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

            var shoppingListMock = local.ShoppingListMockFixture.Create();
            var shoppingListItem = local.ShoppingListItemFixture.AsModelFixture().CreateValid();

            SectionId sectionId = local.CreateSectionIdNotOnList(shoppingListMock.Object);
            var store = local.CreateStore(shoppingListMock.Object, sectionId);

            var shoppingListSection = local.CreateSection(sectionId);

            local.StoreRepositoryMock.SetupFindByAsync(store.Id, store);
            local.ShoppingListSectionFactoryMock.SetupCreateEmpty(sectionId, shoppingListSection);

            // Act
            await service.AddItemToShoppingList(shoppingListMock.Object, shoppingListItem, sectionId, default);

            // Assert
            using (new AssertionScope())
            {
                local.ShoppingListSectionFactoryMock.VerifyCreateEmptyOnce(sectionId);
                shoppingListMock.VerifyAddSectionOnce(shoppingListSection);
                shoppingListMock.VerifyAddItemOnce(shoppingListItem, sectionId);
            }
        }

        [Fact]
        public async Task AddItemToShoppingList_WithSectionAlreadyInShoppingList_ShouldNotAddSectionToShoppingList()
        {
            // Arrange
            var local = new LocalFixture();
            var service = local.CreateService();

            var shoppingListMock = local.ShoppingListMockFixture.Create();
            var shoppingListItem = local.ShoppingListItemFixture.AsModelFixture().CreateValid();

            SectionId sectionId = local.CommonFixture.ChooseRandom(shoppingListMock.Object.Sections).Id;
            var store = local.CreateStore(shoppingListMock.Object, sectionId);

            local.StoreRepositoryMock.SetupFindByAsync(store.Id, store);

            // Act
            await service.AddItemToShoppingList(shoppingListMock.Object, shoppingListItem, sectionId, default);

            // Assert
            using (new AssertionScope())
            {
                shoppingListMock.VerifyAddSectionNever();
                shoppingListMock.VerifyAddItemOnce(shoppingListItem, sectionId);
            }
        }

        #endregion AddItemToShoppingList (internal 2nd)

        private class LocalFixture
        {
            public Fixture Fixture { get; }
            public ShoppingListFixture ShoppingListFixture { get; }
            public ShoppingListMockFixture ShoppingListMockFixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ShoppingListSectionFixture ShoppingListSectionFixture { get; }
            public ShoppingListItemFixture ShoppingListItemFixture { get; }
            public StoreItemAvailabilityFixture StoreItemAvailabilityFixture { get; }
            public StoreItemFixture StoreItemFixture { get; }
            public StoreSectionFixture StoreSectionFixture { get; }
            public StoreFixture StoreFixture { get; }
            public ShoppingListSectionFactoryMock ShoppingListSectionFactoryMock { get; }
            public StoreRepositoryMock StoreRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public ShoppingListItemFactoryMock ShoppingListItemFactoryMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                ShoppingListFixture = new ShoppingListFixture(CommonFixture);
                ShoppingListMockFixture = new ShoppingListMockFixture(CommonFixture, ShoppingListFixture);
                ShoppingListSectionFixture = new ShoppingListSectionFixture(CommonFixture);
                ShoppingListItemFixture = new ShoppingListItemFixture(CommonFixture);
                StoreItemAvailabilityFixture = new StoreItemAvailabilityFixture(CommonFixture);
                StoreItemFixture = new StoreItemFixture(StoreItemAvailabilityFixture, CommonFixture);
                StoreSectionFixture = new StoreSectionFixture(CommonFixture);
                StoreFixture = new StoreFixture(CommonFixture);

                ShoppingListSectionFactoryMock = new ShoppingListSectionFactoryMock(Fixture);
                StoreRepositoryMock = new StoreRepositoryMock(Fixture);
                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                ShoppingListItemFactoryMock = new ShoppingListItemFactoryMock(Fixture);
            }

            public AddItemToShoppingListService CreateService()
            {
                return Fixture.Create<AddItemToShoppingListService>();
            }

            public ShoppingListMock CreateValidShoppingListMock(IStoreItem storeItem)
            {
                var storeId = CommonFixture.ChooseRandom(storeItem.Availabilities).StoreId;

                var listDef = new ShoppingListDefinition
                {
                    StoreId = storeId
                };
                var list = ShoppingListFixture.CreateValid(listDef);
                return new ShoppingListMock(list);
            }

            public ShoppingListMock CreateShoppingListMockWithIncompatibleStore(IStoreItem storeItem)
            {
                var storeIds = storeItem.Availabilities.Select(av => av.StoreId.Value);

                var listDef = new ShoppingListDefinition
                {
                    StoreId = new StoreId(CommonFixture.NextInt(storeIds))
                };
                var list = ShoppingListFixture.CreateValid(listDef);
                return new ShoppingListMock(list);
            }

            public IStore CreateStore(IShoppingList shoppingList, SectionId sectionId)
            {
                var sectionDef = StoreSectionDefinition.FromId(sectionId);
                var section = StoreSectionFixture.Create(sectionDef);

                var storeDef = new StoreDefinition
                {
                    Id = shoppingList.StoreId,
                    Sections = section.ToMonoList()
                };
                return StoreFixture.CreateValid(storeDef);
            }

            public SectionId CreateSectionIdNotInStore(IStore store)
            {
                var sectionIds = store.Sections.Select(s => s.Id.Value);

                return new SectionId(CommonFixture.NextInt(sectionIds));
            }

            public SectionId CreateSectionIdNotOnList(IShoppingList shoppingList)
            {
                var sectinIds = shoppingList.Sections.Select(s => s.Id.Value);

                return new SectionId(CommonFixture.NextInt(sectinIds));
            }

            public IShoppingListSection CreateSection(SectionId sectionId)
            {
                var sectionDef = ShoppingListSectionDefinition.FromId(sectionId);
                return ShoppingListSectionFixture.CreateValid(sectionDef);
            }
        }
    }
}