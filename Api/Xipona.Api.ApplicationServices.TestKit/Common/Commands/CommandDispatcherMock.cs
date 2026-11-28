using Moq.Language.Flow;
using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.ApplicationServices.TestKit.Common.Commands;

public class CommandDispatcherMock : Mock<ICommandDispatcher>
{
    public CommandDispatcherMock(MockBehavior behavior) : base(behavior)
    {
    }

    public ISetup<ICommandDispatcher, Task<T>> SetupDispatchAsync<T>(ICommand<T> command)
    {
        return Setup(m => m.DispatchAsync(It.Is<ICommand<T>>(c => command.IsEquivalentTo(c)),
            It.IsAny<CancellationToken>()));
    }

    public void SetupDispatchAsync<T>(ICommand<T> command, T returnValue)
    {
        Setup(m => m.DispatchAsync(It.Is<ICommand<T>>(c => command.IsEquivalentTo(c)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returnValue);
    }

    public void VerifyDispatchAsync<T>(ICommand<T> command, Func<Times> times)
    {
        Verify(m => m.DispatchAsync(It.Is<ICommand<T>>(c => command.IsEquivalentTo(c)),
            It.IsAny<CancellationToken>()), times);
    }
}