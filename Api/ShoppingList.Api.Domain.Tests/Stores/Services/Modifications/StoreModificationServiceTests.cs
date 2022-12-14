using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Stores.Services.Modifications;

public class StoreModificationServiceTests
{
    public class ModifyAsync
    {
        private readonly ModifyAsyncFixture _fixture;

        public ModifyAsync()
        {
            _fixture = new ModifyAsyncFixture();
        }

        [Fact]
        public async Task ModifyAsync_WithStoreNotFound_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupModify();
            _fixture.SetupNotFindingStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modify);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modify);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.StoreNotFound);
        }

        [Fact]
        public async Task ModifyAsync_WithStoreFound_ShouldModifyStore()
        {
            // Arrange
            _fixture.SetupStoreMock();
            _fixture.SetupModify();
            _fixture.SetupFindingStore();
            _fixture.SetupUpdatingStore();
            _fixture.SetupStoringStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modify);

            // Act
            await sut.ModifyAsync(_fixture.Modify);

            // Assert
            _fixture.VerifyUpdatingStore();
        }

        [Fact]
        public async Task ModifyAsync_WithStoreFound_ShouldStoreStore()
        {
            // Arrange
            _fixture.SetupStoreMock();
            _fixture.SetupModify();
            _fixture.SetupFindingStore();
            _fixture.SetupUpdatingStore();
            _fixture.SetupStoringStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modify);

            // Act
            await sut.ModifyAsync(_fixture.Modify);

            // Assert
            _fixture.VerifyStoringStore();
        }

        private sealed class ModifyAsyncFixture : StoreModificationServiceFixture
        {
            private StoreMock? _storeMock;
            public StoreModification? Modify { get; private set; }

            public void SetupModify()
            {
                Modify = new DomainTestBuilder<StoreModification>().Create();
            }

            public void SetupStoreMock()
            {
                _storeMock = new StoreMock(MockBehavior.Strict, new StoreBuilder().Create());
            }

            public void SetupFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Modify);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                SetupFindingStore(Modify.Id, _storeMock.Object);
            }

            public void SetupNotFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Modify);

                SetupNotFindingStore(Modify.Id);
            }

            public void SetupUpdatingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Modify);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                _storeMock.SetupChangeName(Modify.Name);
                _storeMock.SetupModifySectionsAsync(Modify.Sections, ItemModificationServiceMock.Object,
                    ShoppingListModificationServiceMock.Object);
            }

            public void SetupStoringStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                SetupStoringStore(_storeMock.Object, _storeMock.Object);
            }

            public void VerifyUpdatingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(Modify);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                _storeMock.VerifyChangeName(Modify.Name, Times.Once);
                _storeMock.VerifyModifySectionsAsync(Modify.Sections, ItemModificationServiceMock.Object,
                    ShoppingListModificationServiceMock.Object, Times.Once);
            }

            public void VerifyStoringStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                VerifyStoringStore(_storeMock.Object);
            }
        }
    }

    private abstract class StoreModificationServiceFixture
    {
        private readonly StoreRepositoryMock _storeRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemModificationServiceMock ItemModificationServiceMock = new(MockBehavior.Strict);

        protected readonly ShoppingListModificationServiceMock ShoppingListModificationServiceMock =
            new(MockBehavior.Strict);

        public StoreModificationService CreateSut()
        {
            return new StoreModificationService(
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