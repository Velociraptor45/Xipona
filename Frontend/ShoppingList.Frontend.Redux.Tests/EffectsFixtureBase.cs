﻿using Moq;
using Moq.Contrib.InOrder.Extensions;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests;

public abstract class EffectsFixtureBase
{
    protected readonly ApiClientMock ApiClientMock = new(MockBehavior.Strict);
    protected readonly CommandQueueMock CommandQueueMock = new(MockBehavior.Strict);

    public DispatcherMock DispatcherMock { get; } = new(MockBehavior.Strict);

    protected void SetupDispatchingAction<TAction>(TAction action)
    {
        DispatcherMock
            .SetupInOrder(m => m.Dispatch(It.Is<TAction>(a => a.IsEquivalentTo(action))));
    }

    protected void SetupDispatchingAction<TAction>(Func<TAction, bool> match)
    {
        DispatcherMock
            .SetupInOrder(m => m.Dispatch(It.Is<TAction>(a => match(a))));
    }

    protected void SetupDispatchingAnyAction<TAction>()
    {
        DispatcherMock.SetupInOrder(m => m.Dispatch(It.IsAny<TAction>()));
    }

    protected void SetupDispatchingAction<TAction>() where TAction : new()
    {
        SetupDispatchingAction(new TAction());
    }

    protected void VerifyDispatchingAction<TAction>(TAction action)
    {
        DispatcherMock.Verify(m => m.Dispatch(It.Is<TAction>(a => a.IsEquivalentTo(action))), Times.Once);
    }

    protected void VerifyDispatchingAction<TAction>() where TAction : new()
    {
        VerifyDispatchingAction(new TAction());
    }

    protected void VerifyNotDispatchingAction<TAction>()
    {
        DispatcherMock.Verify(m => m.Dispatch(It.IsAny<TAction>()), Times.Never);
    }
}