using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common.Mocks;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
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
        private readonly CommonFixture commonFixture;
        private readonly ItemCategoryMockFixture itemCategoryMockFixtur;
        private readonly StoreItemMockFixture storeItemMockFixture;
        private readonly ShoppingListMockFixture shoppingListMockFixture;

        public DeleteItemCategoryCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            var shoppingListFixture = new ShoppingListFixture(commonFixture);
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

            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);

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

            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            TransactionGeneratorMock transactionGeneratorMock = new TransactionGeneratorMock(fixture);

            ItemCategoryMock itemCategoryMock = itemCategoryMockFixtur.Create();
            TransactionMock transactionMock = new TransactionMock();

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

            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            TransactionGeneratorMock transactionGeneratorMock = new TransactionGeneratorMock(fixture);

            ItemCategoryMock itemCategoryMock = itemCategoryMockFixtur.Create();
            List<StoreItemMock> storeItemMocks = storeItemMockFixture.CreateMany(3).ToList();
            TransactionMock transactionMock = new TransactionMock();

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

            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            ShoppingListRepositoryMock shoppingListRepositoryMock = new ShoppingListRepositoryMock(fixture);
            TransactionGeneratorMock transactionGeneratorMock = new TransactionGeneratorMock(fixture);

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

            TransactionMock transactionMock = new TransactionMock();

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
                        listMock.VerifyRemoveItemOnce(storeItemMock.Object.Id);
                        shoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
                    }
                }

                itemCategoryRepositoryMock.VerifyStoreAsyncOnce(itemCategoryMock.Object);
                transactionMock.VerifyCommitAsyncOnce();
            }
        }
    }
}