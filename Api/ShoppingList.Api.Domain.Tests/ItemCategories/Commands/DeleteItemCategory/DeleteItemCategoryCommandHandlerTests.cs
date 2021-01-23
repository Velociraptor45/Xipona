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

        public DeleteItemCategoryCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            var shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
            var storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            itemCategoryFixture = new ItemCategoryFixture(commonFixture);
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
            int storeItemsCount = 3;
            var fixture = commonFixture.GetNewFixture();

            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IShoppingListRepository> shoppingListRepositoryMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            Mock<ITransactionGenerator> transactionGeneratorMock = fixture.Freeze<Mock<ITransactionGenerator>>();

            IItemCategory itemCategory = itemCategoryFixture.GetItemCategory(isDeleted: false);
            List<IStoreItem> storeItems = storeItemFixture.GetStoreItems(storeItemsCount, isDeleted: false).ToList();
            Mock<ITransaction> transactionMock = new Mock<ITransaction>();

            var command = fixture.Create<DeleteItemCategoryCommand>();
            var handler = fixture.Create<DeleteItemCategoryCommandHandler>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCategoryId, itemCategory);
            itemRepositoryMock.SetupFindActiveByAsync(command.ItemCategoryId, storeItems);
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

                foreach (var storeItem in storeItems)
                {
                    itemRepositoryMock.Verify(
                        i => i.StoreAsync(
                            It.Is<IStoreItem>(item => item == storeItem),
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
    }
}