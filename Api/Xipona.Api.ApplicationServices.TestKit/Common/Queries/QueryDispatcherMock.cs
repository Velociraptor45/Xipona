﻿using Moq.Language.Flow;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.TestKit.Common.Queries;

public class QueryDispatcherMock : Mock<IQueryDispatcher>
{
    public QueryDispatcherMock(MockBehavior behavior) : base(behavior)
    {
    }

    public ISetup<IQueryDispatcher, Task<T>> SetupDispatchAsync<T>(IQuery<T> query)
    {
        return Setup(m => m.DispatchAsync(
            It.Is<IQuery<T>>(q => q.IsEquivalentTo(query)),
            It.IsAny<CancellationToken>()));
    }

    public void SetupDispatchAsync<T>(IQuery<T> query, T returnValue)
    {
        Setup(m => m.DispatchAsync(
                It.Is<IQuery<T>>(q => q.IsEquivalentTo(query)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyDispatchAsync<T>(IQuery<T> query, Func<Times> times)
    {
        Verify(m => m.DispatchAsync(query, It.IsAny<CancellationToken>()), times);
    }
}