using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ShoppingList.Api.Infrastructure.Tests.Common.Transactions;

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