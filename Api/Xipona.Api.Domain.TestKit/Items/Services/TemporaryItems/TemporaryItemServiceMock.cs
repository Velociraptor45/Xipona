using ProjectHermes.Xipona.Api.Domain.Items.Services.TemporaryItems;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.TemporaryItems;

public class TemporaryItemServiceMock : Mock<ITemporaryItemService>
{
    public TemporaryItemServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void VerifyMakePermanentAsync(PermanentItem permanentItem, Func<Times> times)
    {
        Verify(m => m.MakePermanentAsync(permanentItem), times);
    }

    public void SetupMakePermanentAsync(PermanentItem permanentItem)
    {
        Setup(m => m.MakePermanentAsync(permanentItem)).Returns(Task.CompletedTask);
    }
}