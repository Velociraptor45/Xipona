using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Commands.DeleteItemCategory
{
    public class DeleteItemCategoryCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ItemCategoryFixture itemCategoryFixture;
        private readonly StoreItemMockFixture storeItemMockFixture;
        private readonly ShoppingListMockFixture shoppingListMockFixture;

        public DeleteItemCategoryCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            var shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
            var storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            itemCategoryFixture = new ItemCategoryFixture(commonFixture);
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

            IItemCategory itemCategory = itemCategoryFixture.GetItemCategory(isDeleted: false);
            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            var command = fixture.Create<DeleteItemCategoryCommand>();
            var handler = fixture.Create<DeleteItemCategoryCommandHandler>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCategoryId, itemCategory);
            itemRepositoryMock.SetupFindActiveByAsync(command.ItemCategoryId, Enumerable.Empty<IStoreItem>());
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemCategory.IsDeleted.Should().BeTrue();

                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.IsAny<IShoppingList>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);

                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.IsAny<IStoreItem>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);

                itemCategoryRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IItemCategory>(cat => cat == itemCategory),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

                transactionMock.Verify(
                    i => i.CommitAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);
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

            IItemCategory itemCategory = itemCategoryFixture.GetItemCategory(isDeleted: false);
            List<Mock<IStoreItem>> storeItemMocks = new List<Mock<IStoreItem>>
            {
                new Mock<IStoreItem>(),
                new Mock<IStoreItem>(),
                new Mock<IStoreItem>()
            };
            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            var command = fixture.Create<DeleteItemCategoryCommand>();
            var handler = fixture.Create<DeleteItemCategoryCommandHandler>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCategoryId, itemCategory);
            itemRepositoryMock.SetupFindActiveByAsync(command.ItemCategoryId, storeItemMocks.Select(m => m.Object));
            shoppingListRepositoryMock.SetupFindActiveByAsync(Enumerable.Empty<IShoppingList>());
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemCategory.IsDeleted.Should().BeTrue();

                shoppingListRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.IsAny<IShoppingList>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);

                foreach (var storeItemMock in storeItemMocks)
                {
                    storeItemMock.Verify(
                        i => i.Delete(),
                        Times.Once);

                    itemRepositoryMock.Verify(
                        i => i.StoreAsync(
                            It.Is<IStoreItem>(item => item == storeItemMock.Object),
                            It.IsAny<CancellationToken>()),
                        Times.Once);
                }

                itemCategoryRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IItemCategory>(cat => cat == itemCategory),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

                transactionMock.Verify(
                    i => i.CommitAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);
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

            IItemCategory itemCategory = itemCategoryFixture.GetItemCategory(isDeleted: false);
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

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCategoryId, itemCategory);
            itemRepositoryMock.SetupFindActiveByAsync(command.ItemCategoryId, storeItemMocks.Select(m => m.Object));
            transactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemCategory.IsDeleted.Should().BeTrue();

                foreach (var storeItemMock in storeItemMocks)
                {
                    storeItemMock.VerifyDeleteOnce();

                    itemRepositoryMock.Verify(
                        i => i.StoreAsync(
                            It.Is<IStoreItem>(item => item == storeItemMock.Object),
                            It.IsAny<CancellationToken>()),
                        Times.Once);

                    IEnumerable<ShoppingListMock> affiliatedShoppinListMocks = shoppingLists[storeItemMock];
                    foreach (var listMock in affiliatedShoppinListMocks)
                    {
                        shoppingListRepositoryMock.Verify(
                            i => i.StoreAsync(
                                It.Is<IShoppingList>(l => l == listMock.Object),
                                It.IsAny<CancellationToken>()),
                            Times.Once);
                    }
                }

                itemCategoryRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IItemCategory>(cat => cat == itemCategory),
                        It.IsAny<CancellationToken>()),
                    Times.Once);

                transactionMock.Verify(
                    i => i.CommitAsync(
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}