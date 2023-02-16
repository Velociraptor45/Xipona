using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;

public class CommandQueueMock : Mock<ICommandQueue>
{
    public CommandQueueMock(MockBehavior behavior) : base(behavior)
    {
    }
}