using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Threading;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions
{
    public static class TransactionMockExtensions
    {
        public static void VerifyCommitAsyncOnce(this Mock<ITransaction> mock)
        {
            mock.Verify(
                i => i.CommitAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}