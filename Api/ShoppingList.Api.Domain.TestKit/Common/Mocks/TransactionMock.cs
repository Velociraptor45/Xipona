using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Threading;

namespace ShoppingList.Api.Domain.TestKit.Common.Mocks
{
    public class TransactionMock : Mock<ITransaction>
    {
        public TransactionMock()
        {
        }

        public void VerifyCommitAsyncOnce()
        {
            Verify(
                i => i.CommitAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}