using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class StoreRepositoryMockExtensions
    {
        public static void SetupGetAsync(this Mock<IStoreRepository> mock, IEnumerable<IStore> returnValue)
        {
            mock
                .Setup(i => i.GetAsync(
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(returnValue));
        }

        public static void SetupFindByAsync(this Mock<IStoreRepository> mock, StoreId storeId, IStore returnValue)
        {
            mock
                .Setup(i => i.FindByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public static void SetupFindActiveByAsync(this Mock<IStoreRepository> mock, StoreId storeId,
            IStore returnValue)
        {
            mock
                .Setup(i => i.FindActiveByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(returnValue);
        }

        public static void VerifyFindActiveByAsyncOnce(this Mock<IStoreRepository> mock, StoreId storeId)
        {
            mock.Verify(i => i.FindActiveByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public static void VerifyFindByAsyncOnce(this Mock<IStoreRepository> mock, StoreId storeId)
        {
            mock.Verify(i => i.FindByAsync(
                    It.Is<StoreId>(id => id == storeId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}