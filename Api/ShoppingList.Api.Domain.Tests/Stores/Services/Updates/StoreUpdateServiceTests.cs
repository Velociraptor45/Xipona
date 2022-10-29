using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Stores.Services.Updates;

public class StoreUpdateServiceTests
{
    public class UpdateAsync
    {
        private readonly UpdateAsyncFixture _fixture;

        public UpdateAsync()
        {
            _fixture = new UpdateAsyncFixture();
        }

        [Fact]
        public async Task UpdateAsync_WithStoreNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupUpdate();
            _fixture.SetupNotFindingStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Update);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.Update);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.StoreNotFound);
        }

        [Fact]
        public async Task UpdateAsync_WithStoreFound_ShouldUpdateStore()
        {
            // Arrange
            _fixture.SetupStoreMock();
            _fixture.SetupUpdate();
            _fixture.SetupFindingStore();
            _fixture.SetupUpdatingStore();
            _fixture.SetupStoringStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Update);

            // Act
            await sut.UpdateAsync(_fixture.Update);

            // Assert
            _fixture.VerifyUpdatingStore();
        }

        [Fact]
        public async Task UpdateAsync_WithStoreFound_ShouldStoreStore()
        {
            // Arrange
            _fixture.SetupStoreMock();
            _fixture.SetupUpdate();
            _fixture.SetupFindingStore();
            _fixture.SetupUpdatingStore();
            _fixture.SetupStoringStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Update);

            // Act
            await sut.UpdateAsync(_fixture.Update);

            // Assert
            _fixture.VerifyStoringStore();
        }

        private sealed class UpdateAsyncFixture : StoreUpdateServiceFixture
        {
            private StoreMock? _storeMock;
            public StoreUpdate? Update { get; private set; }

            public void SetupUpdate()
            {
                Update = new DomainTestBuilder<StoreUpdate>().Create();
            }

            public void SetupStoreMock()
            {
                _storeMock = new StoreMock(MockBehavior.Strict, new StoreBuilder().Create());
            }

            public void SetupFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Update);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                SetupFindingStore(Update.Id, _storeMock.Object);
            }

            public void SetupNotFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Update);

                SetupNotFindingStore(Update.Id);
            }

            public void SetupUpdatingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Update);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                _storeMock.SetupChangeName(Update.Name);
                _storeMock.SetupUpdateSectionsAsync(Update.Sections, ItemModificationServiceMock.Object,
                    ShoppingListModificationServiceMock.Object);
            }

            public void SetupStoringStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                SetupStoringStore(_storeMock.Object, _storeMock.Object);
            }

            public void VerifyUpdatingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Update);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                _storeMock.VerifyChangeName(Update.Name, Times.Once);
                _storeMock.VerifyUpdateSectionsAsync(Update.Sections, ItemModificationServiceMock.Object,
                    ShoppingListModificationServiceMock.Object, Times.Once);
            }

            public void VerifyStoringStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                VerifyStoringStore(_storeMock.Object);
            }
        }
    }

    private abstract class StoreUpdateServiceFixture
    {
        private readonly StoreRepositoryMock _storeRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemModificationServiceMock ItemModificationServiceMock = new(MockBehavior.Strict);

        protected readonly ShoppingListModificationServiceMock ShoppingListModificationServiceMock =
            new(MockBehavior.Strict);

        public StoreUpdateService CreateSut()
        {
            return new StoreUpdateService(
                _storeRepositoryMock.Object,
                _ => ItemModificationServiceMock.Object,
                _ => ShoppingListModificationServiceMock.Object,
                default);
        }

        protected void SetupFindingStore(StoreId storeId, IStore store)
        {
            _storeRepositoryMock.SetupFindByAsync(storeId, store);
        }

        protected void SetupNotFindingStore(StoreId storeId)
        {
            _storeRepositoryMock.SetupFindByAsync(storeId, null);
        }

        protected void SetupStoringStore(IStore store, IStore returnValue)
        {
            _storeRepositoryMock.SetupStoreAsync(store, returnValue);
        }

        protected void VerifyStoringStore(IStore store)
        {
            _storeRepositoryMock.VerifyStoreAsync(store, Times.Once);
        }
    }
}