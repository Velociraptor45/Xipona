using Moq;
using ProjectHermes.ShoppingList.Api.Core.Services;

namespace ProjectHermes.ShoppingList.Api.Core.TestKit.Services;

public class DateTimeServiceMock : Mock<IDateTimeService>
{
    public DateTimeServiceMock(MockBehavior behavior) : base(behavior)
    {
        SetupUtcNow(DateTimeOffset.UtcNow);
    }

    public void SetupUtcNow(DateTimeOffset returnValue)
    {
        Setup(m => m.UtcNow).Returns(returnValue);
    }
}