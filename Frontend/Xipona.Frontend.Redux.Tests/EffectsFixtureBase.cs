using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;
using Xipona.Frontend.Redux.Shared.Actions;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.TestTools.Extensions;

namespace Xipona.Frontend.Redux.Tests;

public abstract class EffectsFixtureBase
{
    protected readonly ApiClientMock ApiClientMock = new(MockBehavior.Strict);
    protected readonly CommandQueueMock CommandQueueMock = new(MockBehavior.Strict);

    public DispatcherMock DispatcherMock { get; } = new(MockBehavior.Strict);

    protected void SetupDispatchingAction<TAction>(TAction action, IQueueComponent component)
    {
        DispatcherMock
            .SetupInOrder(m => m.Dispatch(It.Is<TAction>(a => a.IsEquivalentTo(action))), component);
    }

    protected void SetupDispatchingAction<TAction>(Func<TAction, bool> match, IQueueComponent component)
    {
        DispatcherMock
            .SetupInOrder(m => m.Dispatch(It.Is<TAction>(a => match(a))), component);
    }

    protected void SetupDispatchingAction<TAction>(IQueueComponent component) where TAction : new()
    {
        SetupDispatchingAction(new TAction(), component);
    }

    protected void SetupDispatchingAnyAction<TAction>(IQueueComponent component)
    {
        DispatcherMock.SetupInOrder(m => m.Dispatch(It.IsAny<TAction>()), component);
    }

    protected void VerifyNotDispatchingAction<TAction>()
    {
        DispatcherMock.Verify(m => m.Dispatch(It.IsAny<TAction>()), Times.Never);
    }

    public void SetupDispatchingExceptionNotificationAction(IQueueComponent component)
    {
        SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>(component);
    }

    public void SetupDispatchingErrorNotificationAction(IQueueComponent component)
    {
        SetupDispatchingAnyAction<DisplayErrorNotificationAction>(component);
    }
}