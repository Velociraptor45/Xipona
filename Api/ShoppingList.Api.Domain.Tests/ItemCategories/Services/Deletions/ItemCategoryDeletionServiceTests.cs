using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Services.Deletions;

public class ItemCategoryDeletionServiceTests
{
    public class DeleteAsyncTests
    {
        private readonly DeleteAsyncFixture _fixture;

        public DeleteAsyncTests()
        {
            _fixture = new DeleteAsyncFixture();
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
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
                await action.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemCategoryNotFound);
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
        public async Task DeleteAsync_WithNoItemsOfItemCategory_ShouldStoreItemCategory()
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
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldStoreItemCategory()
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
        public async Task DeleteAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreItemCategory()
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
            private ItemCategoryMock _itemCategoryMock;
            private List<StoreItemMock> _storeItemMocks;
            private readonly Dictionary<StoreItemMock, List<ShoppingListMock>> _shoppingListDict = new();

            public ItemCategoryId ItemCategoryId { get; private set; }

            public void SetupItemCategoryId()
            {
                ItemCategoryId = ItemCategoryId.New;
            }

            public void SetupItemCategoryMock()
            {
                _itemCategoryMock = new ItemCategoryMock(ItemCategoryMother.NotDeleted().Create(), MockBehavior.Strict);
            }

            public void SetupStoreItemMocks()
            {
                _storeItemMocks = StoreItemMother.Initial()
                    .CreateMany(2)
                    .Select(i => new StoreItemMock(i, MockBehavior.Strict))
                    .ToList();
            }

            public void SetupShoppingListDict()
            {
                foreach (var storeItemMock in _storeItemMocks)
                {
                    int amount = CommonFixture.NextInt(1, 5);
                    var listMocks = ShoppingListMother.Sections(3)
                        .CreateMany(amount)
                        .Select(list => new ShoppingListMock(list))
                        .ToList();
                    _shoppingListDict.Add(storeItemMock, listMocks);
                }
            }

            #region Mock Setup

            public void SetupDeletingItems()
            {
                foreach (var itemMock in _storeItemMocks)
                {
                    itemMock.SetupDelete();
                }
            }

            public void SetupFindingShoppingLists()
            {
                foreach (var storeItemMock in _shoppingListDict.Keys)
                {
                    var lists = _shoppingListDict[storeItemMock].Select(m => m.Object);

                    ShoppingListRepositoryMock.SetupFindActiveByAsync(storeItemMock.Object.Id, lists);
                }
            }

            public void SetupFindingNoShoppingLists()
            {
                foreach (var storeItemMock in _storeItemMocks)
                {
                    ShoppingListRepositoryMock.SetupFindActiveByAsync(storeItemMock.Object.Id,
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
                ItemCategoryRepositoryMock.SetupFindByAsync(ItemCategoryId, _itemCategoryMock.Object);
            }

            public void SetupFindingNoItemCategory()
            {
                ItemCategoryRepositoryMock.SetupFindByAsync(ItemCategoryId, null);
            }

            public void SetupStoringItemCategory()
            {
                ItemCategoryRepositoryMock.SetupStoreAsync(_itemCategoryMock.Object, _itemCategoryMock.Object);
            }

            public void SetupFindingItems()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(ItemCategoryId, _storeItemMocks.Select(m => m.Object));
            }

            public void SetupFindingNoItems()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(ItemCategoryId, Enumerable.Empty<IStoreItem>());
            }

            public void SetupStoringItem()
            {
                foreach (var item in _storeItemMocks)
                {
                    ItemRepositoryMock.SetupStoreAsync(item.Object, item.Object);
                }
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyDeleteItemCategoryOnce()
            {
                _itemCategoryMock.VerifyDeleteOnce();
            }

            public void VerifyDeletingAllItemsOnce()
            {
                foreach (var storeItemMock in _shoppingListDict.Keys)
                {
                    storeItemMock.VerifyDeleteOnce();
                }
            }

            public void VerifyStoringAllItemsOnce()
            {
                foreach (var storeItemMock in _shoppingListDict.Keys)
                {
                    ItemRepositoryMock.VerifyStoreAsyncOnce(storeItemMock.Object);
                }
            }

            public void VerifyStoringNoItem()
            {
                ItemRepositoryMock.VerifyStoreAsyncNever();
            }

            public void VerifyStoringAllShoppingListsOnce()
            {
                foreach (var storeItemMock in _shoppingListDict.Keys)
                {
                    IEnumerable<ShoppingListMock> affiliatedShoppingListMocks = _shoppingListDict[storeItemMock];
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
                foreach (var storeItemMock in _shoppingListDict.Keys)
                {
                    IEnumerable<ShoppingListMock> affiliatedShoppingListMocks = _shoppingListDict[storeItemMock];
                    foreach (var listMock in affiliatedShoppingListMocks)
                    {
                        listMock.VerifyRemoveItemOnce(storeItemMock.Object.Id);
                    }
                }
            }

            public void VerifyStoringItemCategoryOnce()
            {
                ItemCategoryRepositoryMock.VerifyStoreAsyncOnce(_itemCategoryMock.Object);
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists()
            {
                SetupItemCategoryId();
                SetupItemCategoryMock();
                SetupStoreItemMocks();
                SetupShoppingListDict();
                SetupFindingShoppingLists();
                SetupFindingItemCategory();
                SetupFindingItems();
                SetupDeletingItems();
                SetupStoringShoppingLists();
                SetupStoringItem();
                SetupStoringItemCategory();
            }

            public void SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists()
            {
                SetupItemCategoryId();
                SetupItemCategoryMock();
                SetupStoreItemMocks();
                SetupDeletingItems();
                SetupFindingItemCategory();
                SetupFindingItems();
                SetupFindingNoShoppingLists();
                SetupStoringItem();
                SetupStoringItemCategory();
            }

            public void SetupWithNoItemsOfItemCategory()
            {
                SetupItemCategoryId();
                SetupItemCategoryMock();
                SetupFindingItemCategory();
                SetupFindingNoItems();
                SetupStoringItemCategory();
            }

            #endregion Aggregates
        }
    }

    private abstract class LocalFixture
    {
        protected ItemCategoryRepositoryMock ItemCategoryRepositoryMock;
        protected ItemRepositoryMock ItemRepositoryMock;
        protected ShoppingListRepositoryMock ShoppingListRepositoryMock;
        protected CommonFixture CommonFixture;

        protected LocalFixture()
        {
            CommonFixture = new CommonFixture();
            ItemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
        }

        public ItemCategoryDeletionService CreateSut()
        {
            return new ItemCategoryDeletionService(
                ItemCategoryRepositoryMock.Object,
                ItemRepositoryMock.Object,
                ShoppingListRepositoryMock.Object,
                default);
        }
    }
}