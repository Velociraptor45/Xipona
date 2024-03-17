using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Deletions;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ShoppingLists.Services.Deletions;

public class ShoppingListDeletionServiceTests
{
    public class HardDeleteForStoreAsync
    {
        private readonly HardDeleteForStoreAsyncFixture _fixture = new();

        [Fact]
        public async Task HardDeleteForStoreAsync_WithValidStoreId_ShouldDeleteShoppingList()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupFindingShoppingList();
            _fixture.SetupDeletingShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            await sut.HardDeleteForStoreAsync(_fixture.StoreId.Value);

            // Assert
            _fixture.VerifyDeletingShoppingList();
        }

        [Fact]
        public async Task HardDeleteForStoreAsync_WithInvalidStoreId_ShouldNotDeleteShoppingList()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupNotFindingShoppingList();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            await sut.HardDeleteForStoreAsync(_fixture.StoreId.Value);

            // Assert
            _fixture.VerifyNotDeletingShoppingList();
        }

        private sealed class HardDeleteForStoreAsyncFixture : ShoppingListDeletionServiceFixture
        {
            private Domain.ShoppingLists.Models.ShoppingList? _shoppingList;
            public StoreId? StoreId { get; private set; }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupFindingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                _shoppingList = ShoppingListMother.Initial().Create();
                ShoppingListRepositoryMock.SetupFindActiveByAsync(StoreId.Value, _shoppingList);
            }

            public void SetupNotFindingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                ShoppingListRepositoryMock.SetupFindActiveByAsync(StoreId.Value, null);
            }

            public void SetupDeletingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                ShoppingListRepositoryMock.SetupDeleteAsync(_shoppingList.Id);
            }

            public void VerifyDeletingShoppingList()
            {
                TestPropertyNotSetException.ThrowIfNull(_shoppingList);

                ShoppingListRepositoryMock.VerifyDeleteAsync(_shoppingList.Id, Times.Once);
            }

            public void VerifyNotDeletingShoppingList()
            {
                ShoppingListRepositoryMock.VerifyDeleteAsyncNever();
            }
        }
    }

    private abstract class ShoppingListDeletionServiceFixture
    {
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock = new(MockBehavior.Strict);

        public ShoppingListDeletionService CreateSut()
        {
            return new ShoppingListDeletionService(
                ShoppingListRepositoryMock.Object,
                new Mock<ILogger<ShoppingListDeletionService>>(MockBehavior.Loose).Object);
        }
    }
}