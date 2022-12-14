using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;
using System;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Common.Transactions;

public class TransactionMock : Mock<ITransaction>
{
    public TransactionMock(MockBehavior behavior) : base(behavior)
    {
        Setup(m => m.Dispose());
    }

    public void SetupCommitAsync()
    {
        Setup(m => m.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void VerifyCommitAsync(Func<Times> times)
    {
        Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), times);
    }
}