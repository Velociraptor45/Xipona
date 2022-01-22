using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Core.TestKit.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Common.Mocks;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Commands.DeleteItemCategory
{
    public class DeleteItemCategoryCommandHandlerTests
    {
        private readonly LocalFixture _local;

        public DeleteItemCategoryCommandHandlerTests()
        {
            _local = new LocalFixture();
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            _local.SetupCommandNull();
            var handler = _local.CreateSut();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
        {
            // Arrange
            _local.SetupCommand();
            _local.SetupFindingNoItemCategory();

            var handler = _local.CreateSut();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemCategoryNotFound);
            }
        }

        #region WithNoItemsOfItemCategory

        [Fact]
        public async Task HandleAsync_WithNoItemsOfItemCategory_ShouldReturnTrue()
        {
            // Arrange
            _local.SetupWithNoItemsOfItemCategory();
            var handler = _local.CreateSut();

            // Act
            var result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task HandleAsync_WithNoItemsOfItemCategory_ShouldDeleteItemCategory()
        {
            // Arrange
            _local.SetupWithNoItemsOfItemCategory();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyDeleteItemCategoryOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithNoItemsOfItemCategory_ShouldStoreNoShopppingList()
        {
            // Arrange
            _local.SetupWithNoItemsOfItemCategory();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringNoShoppingList();
            }
        }

        [Fact]
        public async Task HandleAsync_WithNoItemsOfItemCategory_ShouldStoreNoItem()
        {
            // Arrange
            _local.SetupWithNoItemsOfItemCategory();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringNoItem();
            }
        }

        [Fact]
        public async Task HandleAsync_WithNoItemsOfItemCategory_ShouldStoreItemCategory()
        {
            // Arrange
            _local.SetupWithNoItemsOfItemCategory();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringItemCategoryOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithNoItemsOfItemCategory_ShouldCommitTransaction()
        {
            // Arrange
            _local.SetupWithNoItemsOfItemCategory();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyCommittingTransactionOnce();
            }
        }

        #endregion WithNoItemsOfItemCategory

        #region WithSomeItemsOfItemCategoryOnNoActiveShoppingLists

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldReturnTrue()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            var result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldDeleteItemCategory()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyDeleteItemCategoryOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldStoreNoShoppingList()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringNoShoppingList();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldDeleteAllItems()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyDeletingAllItemsOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldStoreAllItems()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringAllItemsOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldStoreItemCategory()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringItemCategoryOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldCommitTransaction()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyCommittingTransactionOnce();
            }
        }

        #endregion WithSomeItemsOfItemCategoryOnNoActiveShoppingLists

        #region WithSomeItemsOfItemCategoryOnActiveShoppingLists

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldReturnTrue()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            var result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreDeleteItemCategory()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyDeleteItemCategoryOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldDeleteAllItems()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyDeletingAllItemsOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreAllItems()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringAllItemsOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldRemoveItemFromShoppingLists()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyRemovingItemFromAllShoppingListsOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreAllShoppingLists()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringAllShoppingListsOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreItemCategory()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyStoringItemCategoryOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldCommitTransaction()
        {
            // Arrange
            _local.SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists();
            var handler = _local.CreateSut();

            // Act
            await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                _local.VerifyCommittingTransactionOnce();
            }
        }

        #endregion WithSomeItemsOfItemCategoryOnActiveShoppingLists

        private class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public ItemCategoryRepositoryMock ItemCategoryRepositoryMock { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; }
            public TransactionGeneratorMock TransactionGeneratorMock { get; }

            public DeleteItemCategoryCommand Command { get; private set; }
            public TransactionMock TransactionMock { get; private set; }
            public ItemCategoryMock ItemCategoryMock { get; private set; }
            public List<StoreItemMock> StoreItemMocks { get; private set; }

            public Dictionary<StoreItemMock, List<ShoppingListMock>> ShoppingListDict { get; } =
                new Dictionary<StoreItemMock, List<ShoppingListMock>>();

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();
                ItemCategoryRepositoryMock = new ItemCategoryRepositoryMock(MockBehavior.Strict);
                ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
                ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
                TransactionGeneratorMock = new TransactionGeneratorMock(MockBehavior.Strict);
            }

            public DeleteItemCategoryCommandHandler CreateSut()
            {
                return new DeleteItemCategoryCommandHandler(
                    ItemCategoryRepositoryMock.Object,
                    ItemRepositoryMock.Object,
                    ShoppingListRepositoryMock.Object,
                    TransactionGeneratorMock.Object);
            }

            public void SetupCommand()
            {
                Command = Fixture.Create<DeleteItemCategoryCommand>();
            }

            public void SetupCommandNull()
            {
                Command = null;
            }

            public void SetupItemCategoryMock()
            {
                ItemCategoryMock = new ItemCategoryMock(ItemCategoryMother.NotDeleted().Create());
            }

            public void SetupTransactionMock()
            {
                TransactionMock = new TransactionMock();
            }

            public void SetupStoreItemMocks()
            {
                StoreItemMocks = StoreItemMother.Initial()
                    .CreateMany(2)
                    .Select(i => new StoreItemMock(i, MockBehavior.Strict))
                    .ToList();
            }

            public void SetupShoppingListDict()
            {
                foreach (var storeItemMock in StoreItemMocks)
                {
                    int amount = CommonFixture.NextInt(1, 5);
                    var listMocks = ShoppingListMother.Sections(3)
                        .CreateMany(amount)
                        .Select(list => new ShoppingListMock(list))
                        .ToList();
                    ShoppingListDict.Add(storeItemMock, listMocks);
                }
            }

            #region Mock Setup

            public void SetupDeletingItems()
            {
                foreach (var itemMock in StoreItemMocks)
                {
                    itemMock.SetupDelete();
                }
            }

            public void SetupFindingShoppingLists()
            {
                foreach (var storeItemMock in ShoppingListDict.Keys)
                {
                    var lists = ShoppingListDict[storeItemMock].Select(m => m.Object);

                    ShoppingListRepositoryMock.SetupFindActiveByAsync(storeItemMock.Object.Id, lists);
                }
            }

            public void SetupFindingNoShoppingLists()
            {
                foreach (var storeItemMock in StoreItemMocks)
                {
                    ShoppingListRepositoryMock.SetupFindActiveByAsync(storeItemMock.Object.Id,
                        Enumerable.Empty<IShoppingList>());
                }
            }

            public void SetupStoringShoppingLists()
            {
                foreach (var list in ShoppingListDict.Values.SelectMany(l => l))
                {
                    ShoppingListRepositoryMock.SetupStoreAsync(list.Object);
                }
            }

            public void SetupFindingItemCategory()
            {
                ItemCategoryRepositoryMock.SetupFindByAsync(Command.ItemCategoryId, ItemCategoryMock.Object);
            }

            public void SetupFindingNoItemCategory()
            {
                ItemCategoryRepositoryMock.SetupFindByAsync(Command.ItemCategoryId, null);
            }

            public void SetupStoringItemCategory()
            {
                ItemCategoryRepositoryMock.SetupStoreAsync(ItemCategoryMock.Object, ItemCategoryMock.Object);
            }

            public void SetupFindingItems()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(Command.ItemCategoryId, StoreItemMocks.Select(m => m.Object));
            }

            public void SetupFindingNoItems()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(Command.ItemCategoryId, Enumerable.Empty<IStoreItem>());
            }

            public void SetupStoringItem()
            {
                foreach (var item in StoreItemMocks)
                {
                    ItemRepositoryMock.SetupStoreAsync(item.Object, item.Object);
                }
            }

            public void SetupGeneratingTransaction()
            {
                TransactionGeneratorMock.SetupGenerateAsync(TransactionMock.Object);
            }

            #endregion Mock Setup

            #region Verify

            public void VerifyDeleteItemCategoryOnce()
            {
                ItemCategoryMock.VerifyDeleteOnce();
            }

            public void VerifyDeletingAllItemsOnce()
            {
                foreach (var storeItemMock in ShoppingListDict.Keys)
                {
                    storeItemMock.VerifyDeleteOnce();
                }
            }

            public void VerifyStoringAllItemsOnce()
            {
                foreach (var storeItemMock in ShoppingListDict.Keys)
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
                foreach (var storeItemMock in ShoppingListDict.Keys)
                {
                    IEnumerable<ShoppingListMock> affiliatedShoppingListMocks = ShoppingListDict[storeItemMock];
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
                foreach (var storeItemMock in ShoppingListDict.Keys)
                {
                    IEnumerable<ShoppingListMock> affiliatedShoppingListMocks = ShoppingListDict[storeItemMock];
                    foreach (var listMock in affiliatedShoppingListMocks)
                    {
                        listMock.VerifyRemoveItemOnce(storeItemMock.Object.Id);
                    }
                }
            }

            public void VerifyStoringItemCategoryOnce()
            {
                ItemCategoryRepositoryMock.VerifyStoreAsyncOnce(ItemCategoryMock.Object);
            }

            public void VerifyCommittingTransactionOnce()
            {
                TransactionMock.VerifyCommitAsyncOnce();
            }

            #endregion Verify

            #region Aggregates

            public void SetupWithSomeItemsOfItemCategoryOnActiveShoppingLists()
            {
                SetupCommand();
                SetupItemCategoryMock();
                SetupStoreItemMocks();
                SetupShoppingListDict();
                SetupFindingShoppingLists();
                SetupTransactionMock();
                SetupFindingItemCategory();
                SetupFindingItems();
                SetupGeneratingTransaction();
                SetupDeletingItems();
                SetupStoringShoppingLists();
                SetupStoringItem();
                SetupStoringItemCategory();
            }

            public void SetupWithSomeItemsOfItemCategoryOnNoActiveShoppingLists()
            {
                SetupCommand();
                SetupItemCategoryMock();
                SetupStoreItemMocks();
                SetupDeletingItems();
                SetupTransactionMock();
                SetupFindingItemCategory();
                SetupFindingItems();
                SetupFindingNoShoppingLists();
                SetupGeneratingTransaction();
                SetupStoringItem();
                SetupStoringItemCategory();
            }

            public void SetupWithNoItemsOfItemCategory()
            {
                SetupCommand();
                SetupItemCategoryMock();
                SetupTransactionMock();
                SetupFindingItemCategory();
                SetupFindingNoItems();
                SetupGeneratingTransaction();
                SetupStoringItemCategory();
            }

            #endregion Aggregates
        }
    }
}