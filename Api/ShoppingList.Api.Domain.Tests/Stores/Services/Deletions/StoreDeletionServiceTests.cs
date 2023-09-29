using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Stores.Services.Deletions;

public class StoreDeletionServiceTests
{
    public class DeleteAsync
    {
        private readonly DeleteAsyncFixture _fixture = new();

        [Fact]
        public async Task DeleteAsync_WithValidStoreId_DeletesStore()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupFindingStore();
            _fixture.SetupDeletingStore();
            _fixture.SetupStoringStore();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteAsync(_fixture.StoreId!.Value);

            // Assert
            _fixture.VerifyDeletingStore();
            _fixture.VerifyStoringStore();
        }

        [Fact]
        public async Task DeleteAsync_WithNotExistingStoreId_ThrowsDomainException()
        {
            // Arrange
            _fixture.SetupStoreId();
            _fixture.SetupNotFindingStore();
            var sut = _fixture.CreateSut();

            // Act
            var func = async () => await sut.DeleteAsync(_fixture.StoreId!.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.StoreNotFound);
        }

        private sealed class DeleteAsyncFixture : StoreDeletionServiceFixture
        {
            private StoreMock? _storeMock;
            public StoreId? StoreId { get; private set; }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                _storeMock = new StoreMock(MockBehavior.Strict, StoreMother.Initial().Create());
                StoreRepositoryMock.SetupFindByAsync(StoreId.Value, _storeMock.Object);
            }

            public void SetupNotFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                StoreRepositoryMock.SetupFindByAsync(StoreId.Value, null);
            }

            public void SetupDeletingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                _storeMock.SetupDelete();
            }

            public void SetupStoringStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                StoreRepositoryMock.SetupStoreAsync(_storeMock.Object, _storeMock.Object);
            }

            public void VerifyDeletingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                _storeMock.VerifyDelete(Times.Once);
            }

            public void VerifyStoringStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                StoreRepositoryMock.VerifyStoreAsync(_storeMock.Object, Times.Once);
            }
        }
    }

    private abstract class StoreDeletionServiceFixture
    {
        protected readonly StoreRepositoryMock StoreRepositoryMock = new(MockBehavior.Strict);

        public StoreDeletionService CreateSut()
        {
            return new StoreDeletionService(_ => StoreRepositoryMock.Object, default);
        }
    }
}