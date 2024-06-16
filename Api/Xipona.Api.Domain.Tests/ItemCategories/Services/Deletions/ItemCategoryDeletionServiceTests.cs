using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Deletions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ItemCategories.Services.Deletions;

public class ItemCategoryDeletionServiceTests
{
    public class DeleteAsyncTests
    {
        private readonly DeleteAsyncFixture _fixture = new();

        [Fact]
        public async Task DeleteAsync_WithInvalidItemCategoryId_ShouldNotThrow()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupFindingNoItemCategory();

            var sut = _fixture.CreateSut();

            // Act
            Func<Task> action = async () => await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().NotThrowAsync();
            }
        }

        #region WithNoItemsOfItemCategory

        [Fact]
        public async Task DeleteAsync_WithNoItemsOfItemCategory_ShouldDeleteItemCategory()
        {
            // Arrange
            _fixture.SetupWithNoItemsOfItemCategory();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyDeleteItemCategoryOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithNoItemsOfItemCategory_ShouldStoreNoShopppingList()
        {
            // Arrange
            _fixture.SetupWithNoItemsOfItemCategory();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringNoShoppingList();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithNoItemsOfItemCategory_ShouldStoreNoItem()
        {
            // Arrange
            _fixture.SetupWithNoItemsOfItemCategory();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringNoItem();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithNoItemsOfItemCategory_ShouldItemCategory()
        {
            // Arrange
            _fixture.SetupWithNoItemsOfItemCategory();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringItemCategoryOnce();
            }
        }

        #endregion WithNoItemsOfItemCategory

        #region WithSomeItemsOfItemCategoryOnNoActiveShoppingLists

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldDeleteItemCategory()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyDeleteItemCategoryOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldStoreNoShoppingList()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringNoShoppingList();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldDeleteAllItems()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyDeletingAllItemsOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldStoreAllItems()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringAllItemsOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldItemCategory()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringItemCategoryOnce();
            }
        }

        #endregion WithSomeItemsOfItemCategoryOnNoActiveShoppingLists

        #region WithSomeItemsOfItemCategoryOnActiveShoppingLists

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreDeleteItemCategory()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyDeleteItemCategoryOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldDeleteAllItems()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyDeletingAllItemsOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreAllItems()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringAllItemsOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldRemoveItemFromShoppingLists()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyRemovingItemFromAllShoppingListsOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreAllShoppingLists()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringAllShoppingListsOnce();
            }
        }

        [Fact]
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldItemCategory()
        {
            // Arrange
            _fixture.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.ItemCategoryId);

            // Assert
            using (new AssertionScope())
            {
                _fixture.VerifyStoringItemCategoryOnce();
            }
        }

        #endregion WithSomeItemsOfItemCategoryOnActiveShoppingLists

        private sealed class DeleteAsyncFixture : LocalFixture
        {
            private ItemCategoryMock? _itemCategoryMock;
            private List<ItemMock>? _itemMocks;
            private readonly Dictionary<ItemMock, List<ShoppingListMock>> _shoppingListDict = new();

            public ItemCategoryId ItemCategoryId { get; private set; }

            public void SetupItemCategoryId()
            {
                ItemCategoryId = ItemCategoryId.New;
            }

            public void SetupItemCategoryMock()
            {
                _itemCategoryMock = new ItemCategoryMock(ItemCategoryMother.NotDeleted().Create(), MockBehavior.Strict);
            }

            public void SetupItemMocks()
            {
                _itemMocks = ItemMother.Initial()
                    .CreateMany(2)
                    .Select(i => new ItemMock(i, MockBehavior.Strict))
                    .ToList();
            }

            public void SetupShoppingListDict()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMocks);
                foreach (var itemMock in _itemMocks)
                {
                    int amount = CommonFixture.NextInt(1, 5);
                    var listMocks = ShoppingListMother.Sections(3)
                        .CreateMany(amount)
                        .Select(list => new ShoppingListMock(list))
                        .ToList();
                    _shoppingListDict.Add(itemMock, listMocks);
                }
            }

            #region Mock Setup

            public void SetupDeletingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMocks);
                foreach (var itemMock in _itemMocks)
                {
                    itemMock.SetupDelete();
                }
            }

            public void SetupDeletingItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);
                _itemCategoryMock.SetupDelete();
            }

            public void SetupFindingShoppingLists()
            {
                foreach (var itemMock in _shoppingListDict.Keys)
                {
                    var lists = _shoppingListDict[itemMock].Select(m => m.Object);

                    ShoppingListRepositoryMock.SetupFindActiveByAsync(itemMock.Object.Id, lists);
                }
            }

            public void SetupFindingNoShoppingLists()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMocks);
                foreach (var itemMock in _itemMocks)
                {
                    ShoppingListRepositoryMock.SetupFindActiveByAsync(itemMock.Object.Id,
                        Enumerable.Empty<IShoppingList>());
                }
            }

            public void SetupStoringShoppingLists()
            {
                foreach (var list in _shoppingListDict.Values.SelectMany(l => l))
                {
                    ShoppingListRepositoryMock.SetupStoreAsync(list.Object);
                }
            }

            public void SetupFindingItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);
                ItemCategoryRepositoryMock.SetupFindActiveByAsync(ItemCategoryId, _itemCategoryMock.Object);
            }

            public void SetupFindingNoItemCategory()
            {
                ItemCategoryRepositoryMock.SetupFindActiveByAsync(ItemCategoryId, null);
            }

            public void SetupStoringItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);
                ItemCategoryRepositoryMock.SetupStoreAsync(_itemCategoryMock.Object, _itemCategoryMock.Object);
            }

            public void SetupFindingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMocks);
                ItemRepositoryMock.SetupFindActiveByAsync(ItemCategoryId, _itemMocks.Select(m => m.Object));
            }

            public void SetupFindingNoItems()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(ItemCategoryId, Enumerable.Empty<IItem>());
            }

            public void SetupStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMocks);
                foreach (var item in _itemMocks)
                {
                    ItemRepositoryMock.SetupStoreAsync(item.Object, item.Object);
                }
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyDeleteItemCategoryOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);
                _itemCategoryMock.VerifyDeleteOnce();
            }

            public void VerifyDeletingAllItemsOnce()
            {
                foreach (var itemMock in _shoppingListDict.Keys)
                {
                    itemMock.VerifyDeleteOnce();
                }
            }

            public void VerifyStoringAllItemsOnce()
            {
                foreach (var itemMock in _shoppingListDict.Keys)
                {
                    ItemRepositoryMock.VerifyStoreAsyncOnce(itemMock.Object);
                }
            }

            public void VerifyStoringNoItem()
            {
                ItemRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyStoringAllShoppingListsOnce()
            {
                foreach (var itemMock in _shoppingListDict.Keys)
                {
                    IEnumerable<ShoppingListMock> affiliatedShoppingListMocks = _shoppingListDict[itemMock];
                    foreach (var listMock in affiliatedShoppingListMocks)
                    {
                        ShoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
                    }
                }
            }

            public void VerifyStoringNoShoppingList()
            {
                ShoppingListRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyRemovingItemFromAllShoppingListsOnce()
            {
                foreach (var itemMock in _shoppingListDict.Keys)
                {
                    IEnumerable<ShoppingListMock> affiliatedShoppingListMocks = _shoppingListDict[itemMock];
                    foreach (var listMock in affiliatedShoppingListMocks)
                    {
                        listMock.VerifyRemoveItemOnce(itemMock.Object.Id);
                    }
                }
            }

            public void VerifyStoringItemCategoryOnce()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryMock);
                ItemCategoryRepositoryMock.VerifyStoreAsyncOnce(_itemCategoryMock.Object);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists()
            {
                SetupItemCategoryId();
                SetupItemCategoryMock();
                SetupItemMocks();
                SetupShoppingListDict();
                SetupFindingShoppingLists();
                SetupFindingItemCategory();
                SetupFindingItems();
                SetupDeletingItems();
                SetupStoringShoppingLists();
                SetupStoringItem();
                SetupDeletingItemCategory();
                SetupStoringItemCategory();
            }

            public void SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists()
            {
                SetupItemCategoryId();
                SetupItemCategoryMock();
                SetupItemMocks();
                SetupDeletingItems();
                SetupFindingItemCategory();
                SetupFindingItems();
                SetupFindingNoShoppingLists();
                SetupStoringItem();
                SetupDeletingItemCategory();
                SetupStoringItemCategory();
            }

            public void SetupWithNoItemsOfItemCategory()
            {
                SetupItemCategoryId();
                SetupItemCategoryMock();
                SetupFindingItemCategory();
                SetupFindingNoItems();
                SetupDeletingItemCategory();
                SetupStoringItemCategory();
            }

            #endregion Aggregates
        }
    }

    private abstract class LocalFixture
    {
        protected readonly ItemCategoryRepositoryMock ItemCategoryRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock = new(MockBehavior.Strict);

        public ItemCategoryDeletionService CreateSut()
        {
            return new ItemCategoryDeletionService(
                ItemCategoryRepositoryMock.Object,
                ItemRepositoryMock.Object,
                ShoppingListRepositoryMock.Object);
        }
    }
}