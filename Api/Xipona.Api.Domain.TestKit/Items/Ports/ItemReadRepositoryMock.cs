using Xipona.Api.Domain.Items.Ports;

namespace Xipona.Api.Domain.TestKit.Items.Ports;

public class ItemReadRepositoryMock : Mock<IItemReadRepository>
{
    public ItemReadRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }
}