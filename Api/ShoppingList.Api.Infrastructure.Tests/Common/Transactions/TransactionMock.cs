using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;
using System;

namespace ShoppingList.Api.Infrastructure.Tests.Common.Transactions;

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