using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Services.Modifications;

public class ManufacturerModificationServiceMock : Mock<IManufacturerModificationService>
{
    public ManufacturerModificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupModifyAsync(ManufacturerModification modification)
    {
        Setup(m => m.ModifyAsync(modification)).Returns(Task.CompletedTask);
    }

    public void VerifyModifyAsync(ManufacturerModification modification, Func<Times> times)
    {
        Verify(m => m.ModifyAsync(modification), times);
    }
}