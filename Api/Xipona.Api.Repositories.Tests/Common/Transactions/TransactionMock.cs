using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;
using System;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Common.Transactions;

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