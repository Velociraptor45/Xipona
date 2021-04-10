using AutoFixture;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.Stores.Ports
{
    public class StoreRepositoryMock
    {
        private readonly Mock<IStoreRepository> mock;

        public StoreRepositoryMock(Mock<IStoreRepository> mock)
        {
            this.mock = mock;
        }

        public StoreRepositoryMock(Fixture fixture)
        {
            mock = fixture.Freeze<Mock<IStoreRepository>>();
        }

        public void SetupGetAsync(IEnumerable<IStore> returnValue)
        {
            mock
                .Setup(i => i.GetAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public void SetupFindByAsync(StoreId storeId, IStore returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public void SetupFindActiveByAsync(StoreId storeId, IStore returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public void VerifyFindActiveByAsyncOnce(StoreId storeId)
        {
            mock.Verify(i => i.FindActiveByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public void VerifyFindByAsyncOnce(StoreId storeId)
        {
            mock.Verify(i => i.FindByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}