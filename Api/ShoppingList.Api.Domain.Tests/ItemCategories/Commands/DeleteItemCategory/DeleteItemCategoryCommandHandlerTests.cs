using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Mocks;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Commands.DeleteItemCategory
{
    public class DeleteItemCategoryCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ItemCategoryMockFixture itemCategoryMockFixtur;
        private readonly StoreItemMockFixture storeItemMockFixture;
        private readonly ShoppingListMockFixture shoppingListMockFixture;

        public DeleteItemCategoryCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            var shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            var shoppingListSectionFixture = new ShoppingListSectionFixture(commonFixture, shoppingListItemFixture);
            var shoppingListFixture = new ShoppingListFixture(shoppingListSectionFixture, commonFixture);
            var storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            var storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            var itemCategoryFixture = new ItemCategoryFixture(commonFixture);
            itemCategoryMockFixtur = new ItemCategoryMockFixture(commonFixture, itemCategoryFixture);
            storeItemMockFixture = new StoreItemMockFixture(commonFixture, storeItemFixture);
            shoppingListMockFixture = new ShoppingListMockFixture(commonFixture, shoppingListFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var handler = fixture.Create<DeleteItemCategoryCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

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
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();

            var command = fixture.Create<DeleteItemCategoryCommand>();
            var handler = fixture.Create<DeleteItemCategoryCommandHandler>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCategoryId, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithNoItemsOfItemCategory_ShouldNotStoreAnyItemsAndDeleteItemCategory()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IShoppingListRepository> shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            Mock<ITransactionGenerator> transactionGeneratorMock = fixture.Freeze<Mock<ITransactionGenerator>>();

            ItemCategoryMock itemCategoryMock = itemCategoryMockFixtur.Create();
            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            var command = fixture.Create<DeleteItemCategoryCommand>();
            var handler = fixture.Create<DeleteItemCategoryCommandHandler>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCategoryId, itemCategoryMock.Object);
            itemRepositoryMock.SetupFindActiveByAsync(command.ItemCategoryId, Enumerable.Empty<IStoreItem>());
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemCategoryMock.VerifyDeleteOnce();

                shoppingListRepositoryMock.VerifyStoreAsyncNever();
                itemRepositoryMock.VerifyStoreAsyncNever();
                itemCategoryRepositoryMock.VerifyStoreAsyncOnce(itemCategoryMock.Object);

                transactionMock.VerifyCommitAsyncOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnNoActiveShoppingLists_ShouldStoreItemsButNoShoppingListsAndDeleteItemCategory()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IShoppingListRepository> shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            Mock<ITransactionGenerator> transactionGeneratorMock = fixture.Freeze<Mock<ITransactionGenerator>>();

            ItemCategoryMock itemCategoryMock = itemCategoryMockFixtur.Create();
            List<StoreItemMock> storeItemMocks = storeItemMockFixture.CreateMany(3).ToList();
            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            var command = fixture.Create<DeleteItemCategoryCommand>();
            var handler = fixture.Create<DeleteItemCategoryCommandHandler>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCategoryId, itemCategoryMock.Object);
            itemRepositoryMock.SetupFindActiveByAsync(command.ItemCategoryId, storeItemMocks.Select(m => m.Object));
            shoppingListRepositoryMock.SetupFindActiveByAsync(Enumerable.Empty<IShoppingList>());
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemCategoryMock.VerifyDeleteOnce();

                shoppingListRepositoryMock.VerifyStoreAsyncNever();

                foreach (var storeItemMock in storeItemMocks)
                {
                    storeItemMock.VerifyDeleteOnce();

                    itemRepositoryMock.VerifyStoreAsyncOnce(storeItemMock.Object);
                }

                itemCategoryRepositoryMock.VerifyStoreAsyncOnce(itemCategoryMock.Object);
                transactionMock.VerifyCommitAsyncOnce();
            }
        }

        [Fact]
        public async Task HandleAsync_WithSomeItemsOfItemCategoryOnActiveShoppingLists_ShouldStoreItemsAndShoppingListsAndDeleteItemCategory()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IShoppingListRepository> shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            Mock<ITransactionGenerator> transactionGeneratorMock = fixture.Freeze<Mock<ITransactionGenerator>>();

            ItemCategoryMock itemCategoryMock = itemCategoryMockFixtur.Create();
            var storeItemMocks = storeItemMockFixture.CreateMany(2).ToList();
            var shoppingLists = new Dictionary<StoreItemMock, List<ShoppingListMock>>();
            foreach (var storeItemMock in storeItemMocks)
            {
                int amount = commonFixture.NextInt(1, 5);
                var listMocks = shoppingListMockFixture.CreateMany(amount).ToList();
                shoppingLists.Add(storeItemMock, listMocks);

                shoppingListRepositoryMock.SetupFindActiveByAsync(storeItemMock.Object.Id,
                    listMocks.Select(m => m.Object));
            }

            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            var command = fixture.Create<DeleteItemCategoryCommand>();
            var handler = fixture.Create<DeleteItemCategoryCommandHandler>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCategoryId, itemCategoryMock.Object);
            itemRepositoryMock.SetupFindActiveByAsync(command.ItemCategoryId, storeItemMocks.Select(m => m.Object));
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemCategoryMock.VerifyDeleteOnce();

                foreach (var storeItemMock in storeItemMocks)
                {
                    storeItemMock.VerifyDeleteOnce();
                    itemRepositoryMock.VerifyStoreAsyncOnce(storeItemMock.Object);

                    IEnumerable<ShoppingListMock> affiliatedShoppingListMocks = shoppingLists[storeItemMock];
                    foreach (var listMock in affiliatedShoppingListMocks)
                    {
                        listMock.VerifyRemoveItemOnce(storeItemMock.Object.Id.ToShoppingListItemId());
                        shoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
                    }
                }

                itemCategoryRepositoryMock.VerifyStoreAsyncOnce(itemCategoryMock.Object);
                transactionMock.VerifyCommitAsyncOnce();
            }
        }
    }
}